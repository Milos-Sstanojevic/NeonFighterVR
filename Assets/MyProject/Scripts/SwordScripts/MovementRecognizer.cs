using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System.IO;

public class MovementRecognizer : MonoBehaviour
{
    [SerializeField] private Transform movementSource;
    [SerializeField] private float newPositionThresholdDistance = 0.05f;
    [SerializeField] private GameObject debugCubePrefab;
    [SerializeField] private bool creationMode = true;
    [SerializeField] private string newGestureName;

    private bool isPressed;
    private bool isMoving = false;
    private List<Vector3> positionsList = new List<Vector3>();
    private List<Gesture> trainingSet = new List<Gesture>();
    private bool canDoGesture;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnPickupWeaponAction(WeaponPickedUp);
        EventManager.Instance.SubscribeToOnStartedDrawingAction(StartedDrawing);
        EventManager.Instance.SubscribeToOnFinishedDrawingAction(FinishedDrawing);
    }

    private void WeaponPickedUp(GameObject weapon, HandController hand)
    {
        canDoGesture = true;
    }

    private void StartedDrawing(HandData hand)
    {
        if (movementSource == null && hand.GetComponent<HandController>().HasWeaponInHand)
        {
            isPressed = true;
            movementSource = hand.transform;
        }
    }

    private void FinishedDrawing(HandData hand)
    {
        if (movementSource == hand.transform)
            isPressed = false;
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnPickupWeaponAction(WeaponPickedUp);
        EventManager.Instance.UnsubscribeFromOnStartedDrawingAction(StartedDrawing);
        EventManager.Instance.UnsubscribeFromOnFinishedDrawingAction(FinishedDrawing);
    }

    private void Start()
    {
        ReadSavedGestures();
    }

    private void ReadSavedGestures()
    {
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (var item in gestureFiles)
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
    }

    private void Update()
    {
        GestureMovement();
    }

    private void GestureMovement()
    {
        if (isPressed && !isMoving && canDoGesture && movementSource != null)
            StartMovement();
        else if (isMoving && !isPressed)
            EndMovement();
        else if (isMoving && isPressed)
            UpdateMovement();
    }

    private void StartMovement()
    {
        isMoving = true;
        positionsList.Clear();
        positionsList.Add(movementSource.position);

        if (debugCubePrefab)
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
    }

    //Ima bug ako je premali pomeraj on ce da zapamti samo jedan element liste i onda ne moze da se orderi gesture, znaci ovo izvrsavati samo kad je positionsList.Count vece od lupam 5 (barem dok se trenira)
    private void EndMovement()
    {
        isMoving = false;
        if (positionsList.Count < 5)
        {
            movementSource = null;
            return;
        }

        Point[] pointArray = new Point[positionsList.Count];

        for (int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        if (creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            SaveGestureInFile(pointArray);
        }
        else
        {
            RecognizeGesture(newGesture);
        }
    }

    private void SaveGestureInFile(Point[] pointArray)
    {
        string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
        Debug.Log(Application.persistentDataPath);
        GestureIO.WriteGesture(pointArray, newGestureName, fileName);
    }

    private void RecognizeGesture(Gesture newGesture)
    {
        Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());

        Debug.Log(result.GestureClass + " " + result.Score);

        if (result.Score > 0.80f)
            EventManager.Instance.OnDidGesture(result, movementSource.gameObject);

        movementSource = null;
    }

    private void UpdateMovement()
    {
        Vector3 lastPosition = positionsList[positionsList.Count - 1];

        if (Vector3.Distance(movementSource.position, lastPosition) < newPositionThresholdDistance)
            return;

        positionsList.Add(movementSource.position);
        if (debugCubePrefab)
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
    }
}

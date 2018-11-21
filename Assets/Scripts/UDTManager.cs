using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class UDTManager : MonoBehaviour, IUserDefinedTargetEventHandler
{
    public GameObject imageTarget;

    UserDefinedTargetBuildingBehaviour udt_targetBuildingBehaviour;
    ObjectTracker objectTracker;
    DataSet dataSet;
    ImageTargetBuilder.FrameQuality udt_FrameQuality;
    int targetCount = 0;

    //active note at the time of calling the text field - ensures the correct note is modified
    //even if no longer tracked

    /// <summary>
    /// active note at the time of calling the text field - ensures the correct note is modified
    /// even if no longer tracked
    /// </summary>
    public string activeNote;

    public ImageTargetBehaviour targetBehaviour;

    private void Start()
    {
        udt_targetBuildingBehaviour = GetComponent<UserDefinedTargetBuildingBehaviour>();
        if(udt_targetBuildingBehaviour)
        {
            udt_targetBuildingBehaviour.RegisterEventHandler(this);
        }
        
    }

    //updates framequality
    public void OnFrameQualityChanged(ImageTargetBuilder.FrameQuality frameQuality)
    {
        udt_FrameQuality = frameQuality;
       
    }

    //target behaviour initialized
    public void OnInitialized()
    {
        objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        if(objectTracker != null)
        {
            //create dataset
            dataSet = objectTracker.CreateDataSet(); //create new dataset
            objectTracker.ActivateDataSet(dataSet);
            
        }
    }

    public void OnNewTrackableSource(TrackableSource trackableSource)
    {
        targetCount++;
        objectTracker.DeactivateDataSet(dataSet);

        //add trackable to dataset
        ImageTargetBehaviour imageTargetCopy = (ImageTargetBehaviour)Instantiate(targetBehaviour);
        imageTargetCopy.gameObject.name = "NOTE " + targetCount;

        //setting the number on a note
        Notes note = imageTargetCopy.GetComponentInChildren<Notes>();
        note.text = "#" + targetCount + "\n";
        
        dataSet.CreateTrackable(trackableSource, imageTargetCopy.gameObject);

        objectTracker.ActivateDataSet(dataSet);
        
        udt_targetBuildingBehaviour.StartScanning();

    }
	
    public void BuildTarget()
    {
        Debug.Log("building new target");
        
        udt_targetBuildingBehaviour.BuildNewTarget("note"+targetCount.ToString(), targetBehaviour.GetSize().x);
        
    }

    public void RemoveTrackable(string name)
    {
        objectTracker.DeactivateDataSet(dataSet);
        objectTracker.Stop();
        ImageTargetBehaviour im = GameObject.Find(name).GetComponent<ImageTargetBehaviour>();
        dataSet.Destroy(im.Trackable, true);
        TrackerManager.Instance.GetStateManager().DestroyTrackableBehavioursForTrackable(im.Trackable, true);
        
        Destroy(im);
        objectTracker.ActivateDataSet(dataSet);
        objectTracker.Start();
        udt_targetBuildingBehaviour.StartScanning();
    }

    public void DestroyObject(GameObject gameObject)
    {
        RemoveTrackable(gameObject.name);
        Destroy(gameObject);
    }

}

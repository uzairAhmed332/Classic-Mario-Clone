using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WB_Vector3
{

	private float x;
	private float y;
	private float z;

	public WB_Vector3() { }
	public WB_Vector3(Vector3 vec3)
	{
		this.x = vec3.x;
		this.y = vec3.y;
		this.z = vec3.z;
	}

	public static implicit operator WB_Vector3(Vector3 vec3)
	{
		return new WB_Vector3(vec3);
	}
	public static explicit operator Vector3(WB_Vector3 wb_vec3)
	{
		return new Vector3(wb_vec3.x, wb_vec3.y, wb_vec3.z);
	}
}

[System.Serializable]
public class WB_Quaternion
{

	private float w;
	private float x;
	private float y;
	private float z;

	public WB_Quaternion() { }
	public WB_Quaternion(Quaternion quat3)
	{
		this.x = quat3.x;
		this.y = quat3.y;
		this.z = quat3.z;
		this.w = quat3.w;
	}

	public static implicit operator WB_Quaternion(Quaternion quat3)
	{
		return new WB_Quaternion(quat3);
	}
	public static explicit operator Quaternion(WB_Quaternion wb_quat3)
	{
		return new Quaternion(wb_quat3.x, wb_quat3.y, wb_quat3.z, wb_quat3.w);
	}
}

[System.Serializable]
public class GhostShot
{
	public float timeMark = 0.0f;       // mark at which the position and rotation are of af a given shot

	private WB_Vector3 _posMark;
	public Vector3 posMark
	{
		get
		{
			if (_posMark == null)
			{
				return Vector3.zero;
			}
			else
			{
				return (Vector3)_posMark;
			}
		}
		set
		{
			_posMark = (WB_Vector3)value;
		}
	}

	private WB_Quaternion _rotMark;
	public Quaternion rotMark
	{
		get
		{
			if (_rotMark == null)
			{
				return Quaternion.identity;
			}
			else
			{
				return (Quaternion)_rotMark;
			}
		}
		set
		{
			_rotMark = (WB_Quaternion)value;
		}
	}

}

public class Ghost : MonoBehaviour
{

	private List<GhostShot> framesList;
	private List<GhostShot> lastReplayList = null;
	private List<GhostShot> lastReplayListActualMario = null;

	GameObject theGhost;

	//Mario theMario;

	private float replayTimescale = 1;
	private int replayIndex = 0;
	private int replayIndexActualMario = 0;
	private bool moveActualMario = false;
	private float recordTime = 0.0f;
	private float replayTime = 0.0f;
	private float replayTimeActualMario = 0.0f;

	//Check whether we should be recording or not
	public bool startRecording = false, recordingFrame = false, playRecording = false;

	// Temp variable For Saving and loading of video. 
	string currentStaticVideoEndPath = "/testVideo";
	string currentDelayedDynamicVideoEndPath = "";

	void FixedUpdate()
	{
		if (startRecording)   //Why its not not working with other demo scene!

		{
			startRecording = false;
			StartRecording();
		}
		else if (recordingFrame) 
		{
			RecordFrame();
		}
		if (lastReplayList != null && playRecording)  //2nd lvl -> null & false
		{
			MoveGhost();
		}
	}

	private void RecordFrame()
	{
		recordTime += Time.smoothDeltaTime * 1000;
		GhostShot newFrame = new GhostShot()
		{
			timeMark = recordTime,
			posMark = this.transform.position,
			rotMark = this.transform.rotation
		};

		framesList.Add(newFrame);
	}

	public void StartRecording()
	{
		Debug.Log("Recording started");
		framesList = new List<GhostShot>();
		replayIndex = 0;
		recordTime = Time.time * 1000;
		recordingFrame = true;
		playRecording = false;
	}

	public void StartRecordingGhost()
	{
		startRecording = true;
	}

	//This is for making new ghost videos
	public void loadFromFile()
	{
		//Check if Ghost file exists. If it does load it
		if (File.Exists(Application.persistentDataPath + currentStaticVideoEndPath))  //Old: Ghost
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + currentStaticVideoEndPath, FileMode.Open);
			Debug.Log("Loading & Playing GhostGhost file from: " + file.Name);
			lastReplayList = (List<GhostShot>)bf.Deserialize(file);
			Debug.Log("Loading: " + lastReplayList.Count);
			file.Close();
			playGhostRecording();
		}
		else
		{
			Debug.Log("No Load/Ghost Found");
		}
	}

	//This is for existing ghost videos for immedaite feedback
	public void loadFromFile(string loadSceneGhostFromLevelManager, bool comingFromPipe = false)
	{

		if (File.Exists(Application.persistentDataPath + loadSceneGhostFromLevelManager))  
		{
			BinaryFormatter bf = new BinaryFormatter();
			if (!comingFromPipe)
			{ //Normal
				FileStream file = File.Open(Application.persistentDataPath + loadSceneGhostFromLevelManager, FileMode.Open);
				Debug.Log("Loading & Playing GhostGhost file from: " + file.Name);
				lastReplayList = (List<GhostShot>)bf.Deserialize(file);
				Debug.Log("Loading: " + lastReplayList.Count);
				file.Close();
			}
			else {
				//Doing this when coming from pipe but scene name is same!
				if (loadSceneGhostFromLevelManager.Equals("World 1-1"))
				{ 
					FileStream file = File.Open(Application.persistentDataPath + Constants.LOAD_LVL1_3_IMMEDAITE_FEEDBACK_VIDEO, FileMode.Open);
					Debug.Log("Loading & Playing GhostGhost file from: " + file.Name);
					lastReplayList = (List<GhostShot>)bf.Deserialize(file);
					Debug.Log("Loading: " + lastReplayList.Count);
					file.Close();
				}
			}
		
			playGhostRecording();
		}
		else
		{
			Debug.Log("No Load/Ghost Found");
		}
	}

	//This is for existing ghost videos for delayed feedback
	public void loadFromFileDelayedFeedback(string ghostfile, string ActualMariofile, bool comingFromPipe = false)
	{

		if (File.Exists(Application.persistentDataPath + ghostfile))
		{
			BinaryFormatter bf = new BinaryFormatter();
			if (!comingFromPipe)
			{ //Normal
				FileStream file1 =  File.Open(Application.persistentDataPath + ghostfile, FileMode.Open);
				FileStream file2 = File.Open(Application.persistentDataPath + ActualMariofile, FileMode.Open);
				Debug.Log("Loading file1 from: " + file1.Name);
				Debug.Log("Loading file2 from: " + file2.Name);
				lastReplayList = (List<GhostShot>)bf.Deserialize(file1);
				lastReplayListActualMario = (List<GhostShot>)bf.Deserialize(file2);
				Debug.Log("lastReplayList file1: " + lastReplayList.Count);
				Debug.Log("lastReplayList file2: " + lastReplayListActualMario.Count);

				file1.Close();
				file2.Close();
			}
			else
			{
				//Doing this when coming from pipe but scene name is same!
				if (ghostfile.Equals("World 1-1"))
				{
					FileStream file = File.Open(Application.persistentDataPath + Constants.LOAD_LVL1_3_IMMEDAITE_FEEDBACK_VIDEO, FileMode.Open);
					Debug.Log("Loading & Playing GhostGhost file from: " + file.Name);
					lastReplayList = (List<GhostShot>)bf.Deserialize(file);
					Debug.Log("Loading: " + lastReplayList.Count);
					file.Close();
				}
			}

			playGhostRecording();
		}
		else
		{
			Debug.Log("No Load/Ghost Found");
		}
	}

	public void StopRecordingGhost()
	{
		recordingFrame = false;
		lastReplayList = new List<GhostShot>(framesList);

		//This will overwrite any previous Save
		//Run function if new highscore achieved or change filename in function
		SaveGhostToFile(); //Save Ghost to file on device/computer

	}

	private void SaveGhostToFile()
	{
		currentDelayedDynamicVideoEndPath = currentSceneName();

		BinaryFormatter bf = new BinaryFormatter();
		//	FileStream file = File.Create(Application.persistentDataPath + currentStaticVideoEndPath);  //For making new videos (Static path) remove this in future when everything path is dynamic!
		FileStream file = File.Create(Application.persistentDataPath + currentDelayedDynamicVideoEndPath);
		Debug.Log("Stopping & Saving Ghost in: "+ Application.persistentDataPath + currentDelayedDynamicVideoEndPath);
		// Write data to disk
		bf.Serialize(file, lastReplayList);
		file.Close();
	}

    private String currentSceneName()
    {
		if (SceneManager.GetActiveScene().name.Equals("World 1-1") && LevelManager.comingFromPipe)
		{//after Bonus level till end
			return Constants.LOAD_LVL1_3_Delayed_FEEDBACK_VIDEO;
		}
		else if (SceneManager.GetActiveScene().name.Equals("World 1-1"))
		{
			return Constants.LOAD_LVL1_1_Delayed_FEEDBACK_VIDEO;
		}
		else if (SceneManager.GetActiveScene().name.Equals("World 1-1 - Underground"))
		{
			return Constants.LOAD_LVL1_2_Delayed_FEEDBACK_VIDEO;
		}
		else {
			return currentStaticVideoEndPath;
		}

	}

    public void playGhostRecording()
	{
		CreateGhost();
		replayIndex = 0;
		if (Constants.isghostModeDelayedOn) {
			replayIndexActualMario = 0;
		}
		playRecording = true;
	}


	private void MoveGhost()
	{
	
		if (Constants.isghostModeImmediateOn)
		{
			replayIndex++;

			if (replayIndex < lastReplayList.Count)
			{
				GhostShot frame = lastReplayList[replayIndex];
				DoLerp(lastReplayList[replayIndex - 1], frame);
				replayTime += Time.smoothDeltaTime * 1000 * replayTimescale;
			}
		}


		if (Constants.isghostModeDelayedOn) {
			replayIndex++;
			if (replayIndex < lastReplayList.Count)
			{
				
				GhostShot frame = lastReplayList[replayIndex];
				DoLerp(lastReplayList[replayIndex - 1], frame);
				replayTime += Time.smoothDeltaTime * 1000 * replayTimescale;
			}
			replayIndexActualMario++;
			if (replayIndexActualMario < lastReplayListActualMario.Count)
            {
				
				GhostShot frame = lastReplayListActualMario[replayIndexActualMario];
				DoLerpActualMario(lastReplayListActualMario[replayIndexActualMario - 1], frame);
                replayTimeActualMario += Time.smoothDeltaTime * 1000 * replayTimescale;
            }
        }
	}

	private void DoLerpActualMario(GhostShot a, GhostShot b)
	{
		if (Constants.isghostModeDelayedOn)
		{

				this.transform.position = Vector3.Slerp(a.posMark, b.posMark, Mathf.Clamp(replayTimeActualMario, a.timeMark, b.timeMark));
				this.transform.rotation = Quaternion.Slerp(a.rotMark, b.rotMark, Mathf.Clamp(replayTimeActualMario, a.timeMark, b.timeMark));
		}

	}

	private void DoLerp(GhostShot a, GhostShot b)
	{
		//For delayed feedback. Playing Actual mario reply & Ghost mario feedback. This will used after level ends. 

			if (GameObject.FindWithTag("Ghost") != null )
			{
				theGhost.transform.position = Vector3.Slerp(a.posMark, b.posMark, Mathf.Clamp(replayTime, a.timeMark, b.timeMark));
				theGhost.transform.rotation = Quaternion.Slerp(a.rotMark, b.rotMark, Mathf.Clamp(replayTime, a.timeMark, b.timeMark));
			}
	}


	private void CreateGhost() 
		//Camera should not follow this
	{
		//Check if ghost exists or not, no reason to destroy and create it everytime.
		if (GameObject.FindWithTag("Ghost") == null)
		{
			theGhost = Instantiate(Resources.Load("MarioGhost", typeof(GameObject))) as GameObject;  //Old == GhostPrefab
			theGhost.gameObject.tag = "Ghost";

			//Disable RigidBody
			//theGhost.GetComponent<Rigidbody>().isKinematic = true;

		}
	}

	//1. Can you run ACtual mario with vectors intead of doing this?Yes... Done:)
	//2. Now run Actual with ghost mario? Yes...Done :)
	//3. Now  Play differnt recording for record Actual mario andof of ghost-->!? Done :)

	//Dont change with immedate feedback recodings, O

}

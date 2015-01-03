using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using Mono.Data.Sqlite;
using UnityEngine.EventSystems;

public class MonoSQLiteConnect : MonoBehaviour {

	#region �ϐ�
	/// <summary>
	/// �f�[�^�x�[�X�t�@�C����
	/// </summary>
	public string FileName = "GameData.db";

	/// <summary>
	/// start�Ŏ����I�ɐڑ�����Ȃ�true
	/// </summary>
	public bool AutoConnect = true;

	/// <summary>
	/// true�ɂ����persistentDataPath����db�t�@�C����ǂݏ�������
	/// windows�̏ꍇ�A�ꏊ�́������ɂȂ�
	/// C:/Users/(UserName)/AppData/LocalLow/(UserName2)/(ProjectName)
	/// 
	/// false�̏ꍇ��ReadOnly�œ��f�B���N�g������ǂݍ���
	/// </summary>
	public bool PersistentDataPath = false;

	/// <summary>
	/// SQLite�Í����p�X���[�h
	/// �������A�p�X���[�h�ݒ肵���ꍇ�̓���͖��m�F
	/// �󕶎���Ȃ�p�X���[�h���g�p���Ȃ�
	/// </summary>
	public string Password = "";

	SqliteConnection con;

	/// <summary>
	/// �ڑ����Ă�����true
	/// </summary>
	[HideInInspector]
	public bool Connected
	{
		get { return con != null; }
	}

	public Text hardText;
	public Text easyJaText;
	public Text easyText1;
	public Text easyText2;
	public Text easyText3;
	public Text easyText4;
	public InputField inputField;
	SqliteDataReader result;
	public bool question = false;
	public bool answer = false;
	public bool collect = false;
	public string spell;
	public GameObject spawnManager;
	public GameObject easyCanvas;
	public GameObject hardCanvas;
	string[] jaWords = new string[4];
	string[] enWords = new string[4];
	System.Random rng = new System.Random();
	#endregion

	void Awake () {
		if(ModeSelect.mode == "easy"){
			hardCanvas.SetActive(false);
		}else if(ModeSelect.mode == "hard") {
			easyCanvas.SetActive(false);
		}
		spawnManager = GameObject.Find("SpawnManager");
	}
	#region Start
	// Use this for initialization
	void Start()
	{
		if (AutoConnect)
		{
			string conString = null;
			if (PersistentDataPath)
			{
				conString = "uri=file:" + Application.persistentDataPath + "/" + FileName + ";";
			}
			else
			{
				conString = "uri=file:" + FileName + ";";
			}
			//if (Password != "")
			//{
			//    conString += "Password=" + Password + ";";
			//}
			// �q���ł݂�
			con = new SqliteConnection(conString);
			con.SetPassword(Password);
			
			// DB�ɐڑ�
			con.Open();
		}
		AutoConnect = true;
	}
	#endregion

	void Update() {
		if(ModeSelect.mode == "easy"){
			if (!question) {
				UpdateEasyWord();
			}
		}else if(ModeSelect.mode == "hard"){
			if (!question) {
				UpdateHardWord();
			}else{
				if(Input.GetKeyDown("return")){
					OkButtonClick();
				}
				if(Input.GetKeyDown("space")){
					PassButtonClick();
				}
			}
			EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
			inputField.OnPointerClick(new PointerEventData(EventSystem.current));
		}
	}

	void UpdateEasyWord() {
		result = ExecuteSQL("SELECT * FROM WORD ORDER BY RANDOM() LIMIT 4;");
		int i = 0;
		while(result.Read()) {
			jaWords[i] = result.GetString(1);
			enWords[i] = result.GetString(2);
			i++;
		}
		int n = jaWords.Length;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			string jaTmp = jaWords[k];
			string enTmp = enWords[k];
			jaWords[k] = jaWords[n];
			enWords[k] = enWords[n];
			jaWords[n] = jaTmp;
			enWords[n] = enTmp;
		}
		int randomInt = Random.Range (0, 4);
		Debug.Log(randomInt);
		easyJaText.text = jaWords[randomInt];
		easyText1.text = enWords[0];
		easyText2.text = enWords[1];
		easyText3.text = enWords[2];
		easyText4.text = enWords[3];

		question = true;
	}

	void UpdateHardWord() {
		result = ExecuteSQL("SELECT * FROM WORD ORDER BY RANDOM() LIMIT 1;");
		while(result.Read()) {
			hardText.text = result.GetString(1);
			spell = result.GetString(2);
		}
		question = true;
	}

	void CheckWord() {
		if(inputField.text == spell){
			spawnManager.SendMessage("SpawnPlayer");
			question = false;
			inputField.text = "";
		}
	}

	public void CheckSelectWord1() {
		if(easyJaText.text == jaWords[0]){
			spawnManager.SendMessage("SpawnPlayer");
			question = false;
		}else{
			spawnManager.SendMessage("SpawnWarg");
		}
	}

	public void CheckSelectWord2() {
		if(easyJaText.text == jaWords[1]){
			spawnManager.SendMessage("SpawnPlayer");
			question = false;
		}else{
			spawnManager.SendMessage("SpawnWarg");
		}
	}

	public void CheckSelectWord3() {
		if(easyJaText.text == jaWords[2]){
			spawnManager.SendMessage("SpawnPlayer");
			question = false;
		}else{
			spawnManager.SendMessage("SpawnWarg");
		}
	}

	public void CheckSelectWord4() {
		if(easyJaText.text == jaWords[3]){
			spawnManager.SendMessage("SpawnPlayer");
			question = false;
		}else{
			spawnManager.SendMessage("SpawnWarg");
		}
	}

	public void OkButtonClick() {
		CheckWord();
		inputField.text = "";
	}

	public void PassButtonClick() {
		UpdateHardWord();
		spawnManager.SendMessage("SpawnWarg");
		inputField.text = "";
	}

	#region ExecuteSQL:SQL���s
	/// <summary>
	/// �C�ӂ�SQL�����s���܂�
	/// SELECT����������m�F���ĂȂ�
	/// </summary>
	/// <param name="SQL">SQL��</param>
	/// <returns>���ʃ��R�[�h�Q</returns>
	public SqliteDataReader ExecuteSQL(string SQL)
	{
		// SQL�R�}���h�I�u�W�F�N�g�̍쐬
		var cmd = new SqliteCommand(SQL, con);
		
		// SQL���s
		var result = cmd.ExecuteReader();
		return result;
	}
	#endregion

	#region Connect:�Đڑ�
	/// <summary>
	/// �Đڑ�
	/// </summary>
	public void Connect()
	{
		Start();
	}
	#endregion

	#region Close:�ؒf
	/// <summary>
	/// DB�ؒf
	/// </summary>
	public void Close()
	{
		con.Close();
		con = null;
	}
	#endregion
}

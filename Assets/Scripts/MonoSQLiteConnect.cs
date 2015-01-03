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

	public Text text;
	public InputField inputField;
	SqliteDataReader result;
	public bool question = false;
	public string spell;
	public GameObject spawnManager;
	#endregion

	void Awake () {
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
		if (!question) {
			UpdateWord();
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

	void UpdateWord() {
		result = ExecuteSQL("SELECT * FROM WORD ORDER BY RANDOM() LIMIT 1;");
		while(result.Read()) {
			text.text = result.GetString(1);
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

	public void OkButtonClick() {
		CheckWord();
		inputField.text = "";
	}

	public void PassButtonClick() {
		UpdateWord();
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

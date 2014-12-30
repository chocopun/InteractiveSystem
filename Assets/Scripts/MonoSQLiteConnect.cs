using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using Mono.Data.Sqlite;

public class MonoSQLiteConnect : MonoBehaviour {

	#region 変数
	/// <summary>
	/// データベースファイル名
	/// </summary>
	public string FileName = "GameData.db";

	/// <summary>
	/// startで自動的に接続するならtrue
	/// </summary>
	public bool AutoConnect = true;

	/// <summary>
	/// trueにするとpersistentDataPathからdbファイルを読み書きする
	/// windowsの場合、場所は↓ここになる
	/// C:/Users/(UserName)/AppData/LocalLow/(UserName2)/(ProjectName)
	/// 
	/// falseの場合はReadOnlyで同ディレクトリから読み込む
	/// </summary>
	public bool PersistentDataPath = false;

	/// <summary>
	/// SQLite暗号化パスワード
	/// ただし、パスワード設定した場合の動作は未確認
	/// 空文字列ならパスワードを使用しない
	/// </summary>
	public string Password = "";

	SqliteConnection con;

	/// <summary>
	/// 接続していたらtrue
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
			// 繋いでみる
			con = new SqliteConnection(conString);
			con.SetPassword(Password);
			
			// DBに接続
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

	#region ExecuteSQL:SQL実行
	/// <summary>
	/// 任意のSQLを実行します
	/// SELECT文しか動作確認してない
	/// </summary>
	/// <param name="SQL">SQL文</param>
	/// <returns>結果レコード群</returns>
	public SqliteDataReader ExecuteSQL(string SQL)
	{
		// SQLコマンドオブジェクトの作成
		var cmd = new SqliteCommand(SQL, con);
		
		// SQL実行
		var result = cmd.ExecuteReader();
		return result;
	}
	#endregion

	#region Connect:再接続
	/// <summary>
	/// 再接続
	/// </summary>
	public void Connect()
	{
		Start();
	}
	#endregion

	#region Close:切断
	/// <summary>
	/// DB切断
	/// </summary>
	public void Close()
	{
		con.Close();
		con = null;
	}
	#endregion
}

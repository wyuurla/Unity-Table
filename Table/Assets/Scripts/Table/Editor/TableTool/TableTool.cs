using UnityEditor;
using UnityEngine;

/**
 * @brief TableTool
 * @detail 테이블 툴.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public class TableTool : EditorWindow
{
    #region - static
    /**
     * 불러온 파일을 게임에서 사용할수 있는 데이터로 만들어 저장할 경로를 리턴한다.
     */
    static public string directory_tableSave { get { return string.Format("{0}/Resources/Table", Application.dataPath); } }
    
    /**
     * 파일이 들어있는 파일경로를 리턴한다.
     */
    static public string directory_tableLoad { get { return string.Format("{0}/../Table/", Application.dataPath); } }

    /**
     * 테이블 스크립트를 생성할 파일 경로를 리턴한다.
     */
    static public string directory_tableScriptCreate { get { return string.Format("{0}/Scripts/Table/Tables", Application.dataPath); } }
    
    /**
     * 테이블 키 스크립트(Table의 id를 enum으로 사용하기 위해서)를 생성하는 파일경로를 리턴한다.
     */
    static public string directory_keynameCreate { get { return string.Format("{0}/Scripts/Table/TableKeyName", Application.dataPath); } }
    
    /**
     * 엑셀 파일에서 찾을 키이름을 리턴한다.
     */
    static public string keyname { get { return "keyname"; } }
    #endregion

    TableEditorDraw_List m_list;        // 테이블 클래스 리스트.
    TableEditorDraw_Info m_info;        // 선택한 테이블 클래스 상세정보.
    TableEditorDraw_Toolbar m_toolbar;  // 툴바.

    TableBake m_tableBake;

    public TableBake tableBake
    {
        get
        {
            if(m_tableBake == null )
            {
                m_tableBake = new TableBake();
            }
            return m_tableBake;
        }
    }
   
    public TableEditorDraw_Toolbar toolbar { get { return m_toolbar; } }
    public TableEditorDraw_List classList { get { return m_list; } }
    public TableEditorDraw_Info info { get { return m_info; } } 
   
    [MenuItem("GameTools/TableTool")]
	static void Init () 
    {
        EditorWindow.GetWindow<TableTool>(false, "TableTool");
    }
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TableTool));
    }  

    private void OnEnable()
    {
        if (null == m_list)
            m_list = new TableEditorDraw_List(this);
        if (null == m_info)
            m_info = new TableEditorDraw_Info(this);
        if (null == m_toolbar)
            m_toolbar = new TableEditorDraw_Toolbar(this);

        m_toolbar.OnEnable();
        m_list.OnEnable();
        m_info.OnEnable();
    }

    private void OnDisable()
    {
        m_toolbar?.OnDisable();
        m_list?.OnDisable();
        m_info?.OnDisable();
    }

    void OnGUI ()
    {
        m_toolbar.Draw();
        EditorGUILayout.BeginHorizontal();
        m_list.Draw();
        m_info.Draw();
        EditorGUILayout.EndHorizontal();
    }

    private void Update()
    {
        if( null != m_tableBake )
            m_tableBake.UpdateLogic();
    }
}
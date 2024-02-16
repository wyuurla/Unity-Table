/**
 * @brief TableEditorDraw
 * @detail 테이블툴을 구성하는 상위클래스.
 * @date 2024-02-15
 * @version 1.0.0
 * @author kij
 */
public class TableEditorDraw
{
    protected TableTool m_tableTool = null;

    public TableEditorDraw(TableTool _tableTool)
    {
        m_tableTool = _tableTool;
    }

    public virtual void Draw()
    {
    }

    public virtual void OnEnable()
    {
    }

    public virtual void OnDisable()
    {
    }
}
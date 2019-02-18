using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Controls;

namespace Book
{
    public class ChineseLocalizationManager : LocalizationManager
    {
        private static Dictionary<string, string> localizationDictionary = new Dictionary<string, string>();

        static ChineseLocalizationManager()
        {
            localizationDictionary.Add("TreeViewDropAfter", "TreeViewDropAfter");
            localizationDictionary.Add("TreeViewDropBefore", "TreeViewDropBefore");
            localizationDictionary.Add("TreeViewDropIn", "TreeViewDropIn");
            localizationDictionary.Add("TreeViewDropRoot", "TreeViewDropRoot");
            localizationDictionary.Add("CommitEdit", "CommitEdit");
            localizationDictionary.Add("CommitCellEdit", "CommitCellEdit");
            localizationDictionary.Add("BeginEdit", "开始编辑");
            localizationDictionary.Add("BeginInsert", "开始初始化");
            localizationDictionary.Add("CancelCellEdit", "CancelCellEdit");
            localizationDictionary.Add("CancelRowEdit", "CancelRowEdit");
            localizationDictionary.Add("Copy", "复制");
            localizationDictionary.Add("Delete", "删除");
            localizationDictionary.Add("MoveLeft", "左移");
            localizationDictionary.Add("MoveRight", "右移");
            localizationDictionary.Add("MoveUp", "上移");
            localizationDictionary.Add("MoveDown", "下移");
            localizationDictionary.Add("MoveNext", "下一个");
            localizationDictionary.Add("MovePrevious", "上一个");
            localizationDictionary.Add("MoveFirst", "第一个");
            localizationDictionary.Add("MoveLast", "最后一个");
            localizationDictionary.Add("MoveHome", "到首页");
            localizationDictionary.Add("MoveEnd", "到最后");
            localizationDictionary.Add("MovePageDown", "下一页");
            localizationDictionary.Add("MovePageUp", "上一页");
            localizationDictionary.Add("MoveTop", "到顶部");
            localizationDictionary.Add("MoveBottom", "到底部");
            localizationDictionary.Add("Paste", "粘贴");
            localizationDictionary.Add("SelectCurrentItem", "选中当前项");
            localizationDictionary.Add("SelectCurrentUnit", "SelectCurrentUnit");
            localizationDictionary.Add("ExtendSelectionToCurrentUnit", "ExtendSelectionToCurrentUnit");
            localizationDictionary.Add("ActivateRow", "ActivateRow");
            localizationDictionary.Add("ExpandHierarchyItem", "ExpandHierarchyItem");
            localizationDictionary.Add("CollapseHierarchyItem", "CollapseHierarchyItem");
            localizationDictionary.Add("Search", "搜索");
            localizationDictionary.Add("SearchByText", "SearchByText");
            localizationDictionary.Add("CloseSearchPanel", "关闭搜索栏");
            localizationDictionary.Add("TogglePinnedRowState", "TogglePinnedRowState");
            localizationDictionary.Add("Minimize", "最小化");
            localizationDictionary.Add("Restore", "恢复");
            localizationDictionary.Add("Maximize", "最大化");
            localizationDictionary.Add("Close", "关闭");
            localizationDictionary.Add("GridViewColumnsSelectionButtonTooltip", "GridViewColumnsSelectionButtonTooltip");
            localizationDictionary.Add("GridViewGroupPanelText", "GridViewGroupPanelText");
            localizationDictionary.Add("GridViewGroupPanelTopTextGrouped", "GridViewGroupPanelTopTextGrouped");
            localizationDictionary.Add("Ok", "确认");
            localizationDictionary.Add("Cancel", "取消");
            localizationDictionary.Add("SelectAll", "全选");
            localizationDictionary.Add("UnselectAll", "取消全选");
            localizationDictionary.Add("ToggleSelectAll", "反选");
        }

        public override string GetStringOverride(string key)
        {
            if (localizationDictionary.TryGetValue(key, out string value))
            {
                return value;
            }
            else
            {
                return base.GetStringOverride(key);
            }
        }
    }
}

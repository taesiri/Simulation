using System.Collections.Generic;

namespace Problem1.TableRows
{
    public class ReportTableRowList
    {
        private readonly List<ReportTableRowClass> _mainList;

        public ReportTableRowList()
        {
            _mainList = new List<ReportTableRowClass>();
        }

        public void PushRow(ReportTableRowClass item)
        {
            _mainList.Add(item);
        }

        public List<ReportTableRowClass> ReturnData()
        {
            return _mainList;
        }
    }
}
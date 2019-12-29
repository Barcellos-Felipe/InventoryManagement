
using System;
using System.Data;
using System.Text;

namespace InventoryManagement
{
    public class Report
    {
        private int _columnMaxLength = 0;
        private StringBuilder _sb = new StringBuilder();
        DatabaseHandler db = new DatabaseHandler();

        public Report(DataTable table)
        {
            foreach (DataColumn c in table.Columns)
            {
                //if the length of the column name is greater than the current max length
                //update the max length
                _columnMaxLength = _columnMaxLength < c.ColumnName.Length ? c.ColumnName.Length : _columnMaxLength;
            }
            //add some padding
            _columnMaxLength = _columnMaxLength + 2;
        }

        public string GenerateReport(DataTable table)
        {
            GenerateHeader(table);

            // Creates the items rows
            foreach (DataRow r in table.Rows)
            {
                foreach (var item in r.ItemArray)
                {
                    int newMaxLength;

                    // Column 'description'
                    if (item == r.ItemArray[2])
                    {
                        newMaxLength = 55;
                    }
                    // Column 'code'
                    else if (item == r.ItemArray[1])
                    {
                        newMaxLength = 22;
                    }
                    // Skips 'idProduct' column
                    else if (item.ToString() == r.ItemArray[0].ToString())
                    {
                        continue;
                    }
                    else
                    {
                        newMaxLength = _columnMaxLength;
                    }

                    int spaces = newMaxLength - item.ToString().Length;
                    int padLeft = spaces / 2 + item.ToString().Length;

                    _sb.AppendFormat("|{0}|", item.ToString().PadLeft(padLeft).PadRight(newMaxLength));
                }
                _sb.AppendLine();

                foreach (DataColumn c in table.Columns)
                {
                    int newMaxLength;

                    if (c.ColumnName == "description")
                    {
                        newMaxLength = 55;
                    }
                    else if (c.ColumnName == "code")
                    {
                        newMaxLength = 22;
                    }
                    // Skips 'idProduct' column
                    else if (c.ColumnName == "idProduct")
                    {
                        continue;
                    }
                    else
                    {
                        newMaxLength = _columnMaxLength;
                    }

                    _sb.AppendFormat("+{0}+", new String('-', newMaxLength));
                }
                _sb.AppendLine();
            }

            // Adds sum of total prices to bottom of report
            _sb.AppendFormat("\nTotal Inventory: R$ {0}", db.GetTotalInventoryPrice());

            return _sb.ToString();
        }

        private void GenerateHeader(DataTable table)
        {
            //create the top row
            foreach (DataColumn c in table.Columns)
            {
                int newMaxLength;

                if (c.ColumnName == "description")
                {
                    newMaxLength = 55;
                }
                else if (c.ColumnName == "code")
                {
                    newMaxLength = 22;
                }
                // Skips 'idProduct' column
                else if (c.ColumnName == "idProduct")
                {
                    continue;
                }
                else
                {
                    newMaxLength = _columnMaxLength;
                }

                int spaces = newMaxLength - c.ColumnName.Length;
                int padLeft = spaces / 2 + c.ColumnName.Length;

                _sb.AppendFormat("+{0}+", new String('-', newMaxLength));
            }
            _sb.AppendLine();

            //create the column names
            foreach (DataColumn c in table.Columns)
            {
                int newMaxLength;

                if (c.ColumnName == "description")
                {
                    newMaxLength = 55;
                }
                else if (c.ColumnName == "code")
                {
                    newMaxLength = 22;
                }
                // Skips 'idProduct' column
                else if (c.ColumnName == "idProduct")
                {
                    continue;
                }
                else
                {
                    newMaxLength = _columnMaxLength;
                }

                int spaces = newMaxLength - c.ColumnName.Length;
                int padLeft = spaces / 2 + c.ColumnName.Length;

                _sb.AppendFormat("|{0}|", c.ColumnName.PadLeft(padLeft).PadRight(newMaxLength));
            }
            _sb.AppendLine();

            //create the bottom of the column headers same as the top
            for (int i = 0; i <= 1; i++)
            {
                foreach (DataColumn c in table.Columns)
                {
                    int newMaxLength;

                    if (c.ColumnName == "description")
                    {
                        newMaxLength = 55;
                    }
                    else if (c.ColumnName == "code")
                    {
                        newMaxLength = 22;
                    }
                    // Skips 'idProduct' column
                    else if (c.ColumnName == "idProduct")
                    {
                        continue;
                    }
                    else
                    {
                        newMaxLength = _columnMaxLength;
                    }

                    int spaces = newMaxLength - c.ColumnName.Length;
                    int padLeft = spaces / 2 + c.ColumnName.Length;

                    _sb.AppendFormat("+{0}+", new String('-', newMaxLength));
                }
                _sb.AppendLine();
            }
        }
    }
}
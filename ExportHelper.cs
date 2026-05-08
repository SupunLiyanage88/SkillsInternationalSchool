using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace SkillsInternationalSchool
{
    public class ExportHelper
    {
        public static void ExportToExcel(DataTable dt, string fileName)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create Excel file using CSV format (compatible with Excel)
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    // Write headers
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sw.Write("\"" + dt.Columns[i].ColumnName + "\"");
                        if (i < dt.Columns.Count - 1)
                            sw.Write(",");
                    }
                    sw.WriteLine();

                    // Write data rows
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            string value = dr[i]?.ToString() ?? "";
                            sw.Write("\"" + value.Replace("\"", "\"\"") + "\"");
                            if (i < dt.Columns.Count - 1)
                                sw.Write(",");
                        }
                        sw.WriteLine();
                    }
                }

                MessageBox.Show($"Data exported successfully to {fileName}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ExportToPDF(DataTable dt, string fileName)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create a simple PDF using basic text format
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine("%-----------Students Report-----------");
                    sw.WriteLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    sw.WriteLine("%");
                    sw.WriteLine();

                    // Write headers
                    foreach (DataColumn col in dt.Columns)
                    {
                        sw.Write(col.ColumnName.PadRight(20));
                    }
                    sw.WriteLine();
                    sw.WriteLine(new string('-', dt.Columns.Count * 20));

                    // Write data rows
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            string value = dr[i]?.ToString() ?? "";
                            sw.Write(value.PadRight(20));
                        }
                        sw.WriteLine();
                    }
                }

                MessageBox.Show($"Data exported successfully to {fileName}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to PDF: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

using Syncfusion.XlsIO;
using System.Collections.Generic;

namespace User.Model
{
    internal static class SaveFile
    {
        public static void SaveXls(List<List<Point3>> chart3Ddata, string data, IApplication application)
        {
            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = "Решение задач оптимизации";

            for (int i = 0; i < chart3Ddata.Count; i++)
            {
                for (int j = 0; j < chart3Ddata[i].Count; j++)
                {
                    worksheet.Range[i + 1, j + 1].Number = chart3Ddata[i][j].Z;
                }
            }

            IChartShape chart = worksheet.Charts.Add();
            chart.DataRange = worksheet.Range[1, 1, chart3Ddata.Count, chart3Ddata[0].Count];
            chart.ChartType = ExcelChartType.Surface_3D;
            chart.IsSeriesInRows = false;

            chart.ChartTitle = "График целевой функции";
            chart.HasLegend = false;

            chart.Rotation = 20;
            chart.Elevation = 15;
            chart.Perspective = 15;

            chart.TopRow = chart3Ddata.Count + 3;
            chart.LeftColumn = 1;
            chart.BottomRow = chart.TopRow + 20;
            chart.RightColumn = chart.LeftColumn + 8;

            worksheet.Range[chart.TopRow, chart.RightColumn + 1].WrapText = false;
            worksheet.Range[chart.TopRow, chart.RightColumn + 1]
                .CellStyle.VerticalAlignment = (ExcelVAlign)ExcelVerticalAlignment.TopCentered;
            worksheet.Range[chart.TopRow, chart.RightColumn + 1].AutofitColumns();
            worksheet.Range[chart.TopRow, chart.RightColumn + 1].AutofitRows();
            worksheet.Range[chart.TopRow, chart.RightColumn + 3].Text = data;
        }
    }
}

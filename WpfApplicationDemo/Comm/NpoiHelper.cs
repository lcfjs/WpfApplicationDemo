/********************************************************************************
** auth： lichaofeng
** date： 2017/2/21 14:34:08
** desc： 
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NPOI;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace WinformNpoiDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class NpoiHelper
    {
        /// <summary>
        /// 读取Excel的数据至DataTable(多个Sheet对应多张表)
        /// 自动过滤尾部的空行空列
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isFilterEmptyRow">是否过滤空行</param>
        /// <returns></returns>
        public static DataSet ReadToTable(string fileName, bool isFilterEmptyRow = false)
        {
            DataSet ds = new DataSet();

            IWorkbook workbook = null; //工作薄
            ISheet sheet = null; //工作表
            IRow row = null;
            ICell cell = null;
            int emptyCellCount = 0; //用于计数空单元格

            if (File.Exists(fileName))
            {
                //打开文件
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    //excel2003
                    //if (fileName.EndsWith(".xls"))
                    //    workbook = new HSSFWorkbook(fs);
                    //else if (fileName.EndsWith(".xlsx")) //excel2007+
                    //    workbook = new XSSFWorkbook(fs);

                    //自动识别
                    workbook = WorkbookFactory.Create(fs);
                    int sheetCount = workbook.NumberOfSheets; //工作表数量
                    for (int i = 0; i < sheetCount; i++)
                    {
                        DataTable table = new DataTable(workbook.GetSheetName(i));
                        sheet = workbook.GetSheetAt(i);
                        if (sheet != null)
                        {
                            //返回物理定义的行数（不是工作表中的行数）
                            int rowCount = sheet.PhysicalNumberOfRows;
                            for (int j = sheet.FirstRowNum; j < rowCount; j++)
                            {
                                row = sheet.GetRow(j);
                                if (row != null)
                                {
                                    //构建表列
                                    if (table.Columns.Count == 0)
                                    {
                                        //空列？row.FirstCellNum？
                                        for (int k = 0; k < row.LastCellNum; k++)
                                        {
                                            cell = row.GetCell(k);
                                            if (string.IsNullOrEmpty(cell.StringCellValue.Trim())) continue;
                                            DataColumn col = new DataColumn(cell.StringCellValue);
                                            table.Columns.Add(col);
                                        }
                                    }
                                    else
                                    {
                                        emptyCellCount = 0;//重置计数器
                                        DataRow dr = table.NewRow();
                                        for (int k = 0; k < row.LastCellNum; k++)
                                        {
                                            cell = row.GetCell(k);
                                            if (cell != null && k < table.Columns.Count)
                                            {
                                                //可能前面的列会出现空列
                                                int index = k;//k - row.FirstCellNum
                                                //公式
                                                if (cell.CellType == CellType.Formula)
                                                {
                                                    IFormulaEvaluator eval = WorkbookFactory.CreateFormulaEvaluator(workbook);
                                                    CellValue val = eval.Evaluate(cell);
                                                    if (val.CellType == CellType.Numeric)
                                                        dr[index] = cell.NumericCellValue;
                                                    else
                                                        dr[index] = cell.StringCellValue;
                                                }
                                                else if (cell.CellType == CellType.Numeric) //日期或数字
                                                {
                                                    if (DateUtil.IsCellDateFormatted(cell))
                                                        dr[index] = cell.DateCellValue;
                                                    else
                                                        dr[index] = cell.NumericCellValue;
                                                }
                                                else
                                                    dr[index] = cell.StringCellValue;
                                                //空内容的单元格
                                                if (string.IsNullOrEmpty(dr[index] + ""))
                                                    emptyCellCount++;
                                            }
                                            //else
                                            //{
                                            //    emptyCellCount++;
                                            //}
                                        }
                                        if (table.Columns.Count <= emptyCellCount && isFilterEmptyRow)
                                        { }
                                        else
                                            table.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        ds.Tables.Add(table);
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// 将数据导出到Excel
        /// </summary>
        /// <param name="table">要导出的数据表</param>
        /// <param name="saveFileName">要保存的文件完整名称</param>
        /// <param name="isShowRownumber">是否显示行号</param>
        /// <param name="isShowBorder">是否给每个单元格加边框</param>
        /// <returns></returns>
        public static int Export(DataTable table, string saveFileName, bool isShowRownumber = true, bool isShowBorder = false)
        {
            if (table == null) return 0;

            //工作薄
            IWorkbook workbook = null;
            //工作表
            ISheet sheet = null; //2003单个工作表最多只支持65536行数据
            IRow row = null;
            ICell cell = null;

            int exportRowCount = 0; //导出数据总数
            int sheetMaxRow = 0; //单个sheet最大行数
            int rowNumber = 0; //行号
            int rowIndex = 0; //写数据的行下标
            int colOffset = 0;
            int colCount = table.Columns.Count; //列数量
            if (isShowRownumber) colOffset = 1;

            //样式
            ICellStyle borderStyle = null;

            MemoryStream ms = new MemoryStream();

            //workbook = WorkbookFactory.Create(null);
            // excel2003
            if (saveFileName.EndsWith(".xls"))
            {
                workbook = new HSSFWorkbook();
                sheetMaxRow = 65536 - 1; //减去标题行
            }
            else if (saveFileName.EndsWith(".xlsx")) //excel2007+
            {
                workbook = new XSSFWorkbook();
                sheetMaxRow = 1048576 - 1;
            }
            //sheet数量,至少一个sheet
            int sheetCount = table.Rows.Count % sheetMaxRow == 0 ? table.Rows.Count / sheetMaxRow : table.Rows.Count / sheetMaxRow + 1;
            if (sheetCount < 1) sheetCount = 1;

            if (isShowBorder)
            {
                borderStyle = workbook.CreateCellStyle();
                //borderStyle.Alignment = HorizontalAlignment.Center;
                borderStyle.BorderLeft = BorderStyle.Thin;
                borderStyle.BorderTop = BorderStyle.Thin;
                borderStyle.BorderRight = BorderStyle.Thin;
                borderStyle.BorderBottom = BorderStyle.Thin;
            }

            string sheetName = table.TableName;
            //考虑单个Sheet行数上限
            for (int k = 0; k < sheetCount; k++)
            {
                rowIndex = 0; //行下标需要重置
                //rowNumber = 0; //行号重置
                sheet = workbook.CreateSheet(k == 0 ? sheetName : sheetName + "(" + k + ")");
                //标题行
                row = sheet.CreateRow(0);
                //行号列
                if (isShowRownumber)
                {
                    cell = row.CreateCell(0);
                    cell.SetCellValue("序号");
                    cell.CellStyle = borderStyle;
                }
                for (int i = 0; i < colCount; i++)
                {
                    cell = row.CreateCell(i + colOffset);
                    cell.SetCellValue(table.Columns[i].ColumnName);
                    cell.CellStyle = borderStyle;
                }

                int rowMax = table.Rows.Count > sheetMaxRow * (k + 1) ?  sheetMaxRow * (k + 1) : table.Rows.Count;
                //数据行
                for (int i = sheetMaxRow * k; i < rowMax; i++)
                {
                    rowIndex++;
                    exportRowCount++;
                    rowNumber++;
                    row = sheet.CreateRow(rowIndex);

                    if (isShowRownumber)
                    {
                        cell = row.CreateCell(0);
                        cell.SetCellValue(rowNumber);
                        cell.CellStyle = borderStyle;
                    }
                    for (int j = 0; j < colCount; j++)
                    {
                        cell = row.CreateCell(j + colOffset);
                        cell.SetCellValue(table.Rows[i][j] + "");
                        cell.CellStyle = borderStyle;
                    }
                }
            }


            FileStream fs = new FileStream(saveFileName, FileMode.Create, FileAccess.ReadWrite);
            workbook.Write(fs);
            workbook.Close();
            fs.Close();

            return exportRowCount;
        }
    }
}

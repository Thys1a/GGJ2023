using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public static class CSVUtil
{
    public static DataTable OpenCSV(string filePath)//��csv��ȡ���ݷ���table
    {
        DataTable dt = new DataTable();
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
            {
                //��¼ÿ�ζ�ȡ��һ�м�¼
                string strLine = "";
                //��¼ÿ�м�¼�еĸ��ֶ�����
                string[] aryLine = null;
                string[] tableHead = null;
                //��ʾ����
                int columnCount = 0;
                //��ʾ�Ƿ��Ƕ�ȡ�ĵ�һ��
                bool IsFirst = true;
                //���ж�ȡCSV�е�����
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (IsFirst == true)
                    {
                        tableHead = strLine.Split(',');
                        IsFirst = false;
                        columnCount = tableHead.Length;
                        //������
                        for (int i = 0; i < columnCount; i++)
                        {
                            DataColumn dc = new DataColumn(tableHead[i]);
                            dt.Columns.Add(dc);
                        }
                    }
                    else
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (aryLine != null && aryLine.Length > 0)
                {
                    dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                }
                sr.Close();
                fs.Close();
                return dt;
            }
        }
    }

}

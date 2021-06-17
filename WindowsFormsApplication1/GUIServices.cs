using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Distar
{
    public class GUIServices
    {
        /** IMPRESION DE REPORTE **/
        StringFormat strFormat;
        private ArrayList arrColumnLefts = new ArrayList();
        private ArrayList arrColumnWidths = new ArrayList();
        string strPage = "";
        int page_count = 1;
        int iCellHeight = 0;
        int iTotalWidth = 0;
        int iRow = 0;
        Boolean bFirstPage = false;
        Boolean bNewPage = false;
        int iHeaderHeight = 0;

        public static Form giveMeForms(string formName)
        {
            Assembly asm = Assembly.GetEntryAssembly();
            Type formtype = asm.GetType(formName);
            Form f = (Form)Activator.CreateInstance(formtype);
            return f;
        }

        public static void giveMeAlerts(string msg, string caption, MessageBoxButtons buttons)
        {
            MessageBox.Show(msg, caption, buttons);
        }

        public static DialogResult giveMeAlertsWithAction(string msg, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(msg, caption, buttons);
        }

        public void setNuevaImpresion()
        {
            arrColumnLefts = new ArrayList();
            arrColumnWidths = new ArrayList();
            iCellHeight = 0;
            iTotalWidth = 0;
            iRow = 0;
            bFirstPage = false;
            bNewPage = false;
            iHeaderHeight = 0;
            strPage = "";
            page_count = 1;
        }

        public void iniciarImpresion(DataGridView dgv, object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Near;
            strFormat.LineAlignment = StringAlignment.Center;
            strFormat.Trimming = StringTrimming.EllipsisCharacter;
            arrColumnLefts.Clear();
            arrColumnWidths.Clear();
            iCellHeight = 0;
            iRow = 0;
            bFirstPage = true;
            bNewPage = true;
            // Calcular total de ancho
            iTotalWidth = 0;
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                iTotalWidth += item.Width;
            }
        }

        private string getPage()
        {
            return strPage;
        }

        private void setPage(int page)
        {
            strPage = "Página " + page.ToString();
        }

        public void imprimirReporte(DataGridView dgv, string reporte_nombre, object sender, System.Drawing.Printing.PrintPageEventArgs e, Distar_EntidadesNegocio.Usuario userLog)
        {
            try
            {
                string strReport = "Reporte: " + reporte_nombre + " - "+ userLog.apellido + ", " + userLog.nombre;
                int iLeftMargin = e.MarginBounds.Left;
                int iTopMargin = e.MarginBounds.Top;
                // Si hay más páginas para imprimir o no
                bool bMorePagesToPrint = false;
                int iTmpWidth = 0;
                if (bFirstPage)
                {
                    setPage(page_count++);
                    foreach (DataGridViewColumn GridCol in dgv.Columns)
                    {
                        iTmpWidth = System.Convert.ToInt32(Math.Floor(System.Convert.ToDouble(System.Convert.ToDouble(GridCol.Width) / System.Convert.ToDouble(iTotalWidth) * System.Convert.ToDouble(iTotalWidth) * (System.Convert.ToDouble(e.MarginBounds.Width) / System.Convert.ToDouble(iTotalWidth)))));
                        iHeaderHeight = System.Convert.ToInt32(e.Graphics.MeasureString(GridCol.HeaderText, GridCol.InheritedStyle.Font, iTmpWidth).Height) + 11;
                        // Save width and height of headres
                        arrColumnLefts.Add(iLeftMargin);
                        arrColumnWidths.Add(iTmpWidth);
                        iLeftMargin += iTmpWidth;
                    }
                }
                // Repetir mientras que la grilla tenga filas a imprimir
                while (iRow <= dgv.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = dgv.Rows[iRow];
                    // Alto de celda
                    iCellHeight = GridRow.Height + 5;
                    int iCount = 0;
                    // Ver si la página actual permite más filas
                    if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                    {
                        bNewPage = true;
                        bFirstPage = false;
                        bMorePagesToPrint = true;
                        break;
                    }
                    else
                    {
                        if (bNewPage)
                        {
                            // Cabecera
                            e.Graphics.DrawString(strReport, new Font(dgv.Font, FontStyle.Bold), Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top - e.Graphics.MeasureString(strReport, new Font(dgv.Font, FontStyle.Bold), e.MarginBounds.Width).Height - 13);
                            String strDate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
                            // Fecha y Usuario
                            e.Graphics.DrawString(strDate, new Font(dgv.Font, FontStyle.Bold), Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width - e.Graphics.MeasureString(strDate, new Font(dgv.Font, FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top - e.Graphics.MeasureString(strReport, new Font(new Font(dgv.Font, FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 13);
                            e.Graphics.DrawString(getPage(), new Font(dgv.Font, FontStyle.Bold), Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width - e.Graphics.MeasureString(getPage(), new Font(dgv.Font, FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Bottom);
                            // Columnas
                            iTopMargin = e.MarginBounds.Top;
                            foreach (DataGridViewColumn GridCol in dgv.Columns)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle(System.Convert.ToInt32(arrColumnLefts[iCount]), iTopMargin, System.Convert.ToInt32(arrColumnWidths[iCount]), iHeaderHeight));
                                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(System.Convert.ToInt32(arrColumnLefts[iCount]), iTopMargin, System.Convert.ToInt32(arrColumnWidths[iCount]), iHeaderHeight));
                                e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font, new SolidBrush(GridCol.InheritedStyle.ForeColor), new RectangleF(System.Convert.ToInt32(arrColumnLefts[iCount]), iTopMargin, System.Convert.ToInt32(arrColumnWidths[iCount]), iHeaderHeight), strFormat);
                                iCount += 1;
                            }
                            bNewPage = false;
                            iTopMargin += iHeaderHeight;
                        }
                        iCount = 0;
                        // Contenido de columnas
                        foreach (DataGridViewCell Cel in GridRow.Cells)
                        {
                            if (Cel.Value != null)
                                e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font, new SolidBrush(Cel.InheritedStyle.ForeColor), new RectangleF(System.Convert.ToInt32(arrColumnLefts[iCount]), System.Convert.ToSingle(iTopMargin), System.Convert.ToInt32(arrColumnWidths[iCount]), System.Convert.ToSingle(iCellHeight)), strFormat);
                            // Borde de celdas
                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(System.Convert.ToInt32(arrColumnLefts[iCount]), iTopMargin, System.Convert.ToInt32(arrColumnWidths[iCount]), iCellHeight));
                            iCount += 1;
                        }
                    }
                    iRow += 1;
                    iTopMargin += iCellHeight;
                }
                // Si tiene más filas, hacer otra página
                if (bMorePagesToPrint)
                {
                    setPage(page_count++);
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

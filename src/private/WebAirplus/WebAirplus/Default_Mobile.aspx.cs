using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAirplus;
using System.Drawing;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Services;

namespace WebAirplus
{
    public partial class Default_Mobile : System.Web.UI.Page
    {
        public static class GuestValues
        {
            public static List<string> textFields = new List<string>();

            static GuestValues()
            {

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet dataset = Datalayer.GetUserList(Convert.ToString(Session["UserName"]));
            DataTable data = dataset.Tables[0];
            DataTable statuscode_data = dataset.Tables[1];


            
            string[] topvalues = { "FullName", "Check In", "Check Out", "Requested Check In", "Requested Check Out", "Check Out Cleaning", "Remarks", "Status" };
            
            int rowcount = data.Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                Table table = new Table();
                table.ID = "GuestTable" + Convert.ToString(i);
                foreach (string s in topvalues)
                {   
                    table.Width = Unit.Percentage(100);
                    TableCell c = new TableCell();
                    Label l = new Label();
                    TableRow AllRow = new TableRow();
                    l.ID = "toplab" + Convert.ToString(i)+s;
                    l.Text = s;
                    l.Attributes.CssStyle.Add("font-wight", "bold");
                    l.Attributes.CssStyle.Add("color", "Black");
                    l.Attributes.CssStyle.Add("background-color", "Yellow");
                    l.Attributes.CssStyle.Add("padding-left", "5px");
                    l.Attributes.CssStyle.Add("padding-right", "5px");
                    c.Controls.Add(l);
                    c.BorderWidth = new Unit(1);
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderColor = Color.Black;
                    c.BackColor = Color.Yellow;
                    c.ForeColor = Color.Black;

                    AllRow.Cells.Add(c);

                    TableCell tcell = new TableCell();
                    Label ldata = new Label();
                    ldata.ID = "ldata_" + Convert.ToString(i) + "_" + s;
                    ldata.Attributes.CssStyle.Add("color", "Black");
                    ldata.Attributes.CssStyle.Add("background-color", "White");
                    ldata.Attributes.CssStyle.Add("padding-left", "5px");
                    ldata.Attributes.CssStyle.Add("padding-right", "5px");
                    tcell.Controls.Add(ldata);
                    tcell.BorderWidth = new Unit(1);
                    tcell.BorderStyle = BorderStyle.Solid;
                    tcell.BorderColor = Color.Black;
                    tcell.BackColor = Color.White;
                    tcell.ForeColor = Color.Black;

                    AllRow.Cells.Add(tcell);
                    table.Rows.Add(AllRow);                    
                }
                GuestHolder.Controls.Add(table);
            }
            int rowid = 0;
            foreach (DataRow dr in data.Rows)
            {
                int[] sequence = { 3, 7, 8, 5, 6, 10, 11, 9 };
                string tablename = "GuestTable" + Convert.ToString(rowid);
                var tab = GuestHolder.Parent;
                var parenttab = tab.FindControl(tablename);
                for (int i = 0; i < sequence.Length; i++)
                {
                    int selitem = sequence[i];
                    string s = topvalues[i];
                    string controlname = "ldata_" + Convert.ToString(rowid) + "_" + s;
                    Label labcontrol = parenttab.FindControl(controlname) as Label;
                    labcontrol.Text = Convert.ToString(dr[selitem]);
                }
                rowid++;
            }
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;

namespace WebAirplus
{
    public partial class Settings : System.Web.UI.Page
    {
        protected void GeneratePage()
        {
            DataSet dataset = Datalayer.GetSettings(Convert.ToString(Session["UserName"]));
            DataTable data = dataset.Tables[0];
            Table table = new Table();
            table.ID = "settingTable";
            string[] topvalues = { "FullName", "FirstName", "LastName", "Age", "Email", "Phone" };
            string[] repvalues = { "ListingId", "PropertyAddress", "ICAL URL" };
            int i = 0;
            foreach (string s in topvalues)
            {
                string id = s.IndexOf(' ') > 0 ? String.Join("", s.Split(' ')) : s;
                TableCell c = new TableCell();
                Label l = new Label();
                TableRow AllRow = new TableRow();
                l.ID = "toplab" + s;
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
                TextBox ldata = new TextBox();
                ldata.ID = "ldata_" + Convert.ToString(i) + "_" + s;
                ldata.Text = data.Rows[0][i] is DBNull ? "" : Convert.ToString(data.Rows[0][i]);
                ldata.Attributes.CssStyle.Add("color", "Black");
                ldata.Attributes.CssStyle.Add("background-color", "White");
                ldata.Attributes.CssStyle.Add("padding-left", "5px");
                ldata.Attributes.CssStyle.Add("padding-right", "5px");
                ldata.Enabled = false;
                tcell.Controls.Add(ldata);
                tcell.BorderWidth = new Unit(1);
                tcell.BorderStyle = BorderStyle.Solid;
                tcell.BorderColor = Color.Black;
                tcell.BackColor = Color.White;
                tcell.ForeColor = Color.Black;
                i++;
                AllRow.Cells.Add(tcell);
                table.Rows.Add(AllRow);
            }
            foreach (DataRow r in data.Rows)
            {
                int j = Convert.ToInt32(r[6]);
                i=7;
                foreach (string s in repvalues)
                {
                    string id = s.IndexOf(' ') > 0 ? String.Join("", s.Split(' ')) : s;
                    TableCell c = new TableCell();
                    Label l = new Label();
                    TableRow AllRow = new TableRow();
                    l.ID = "toplab" + s;
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
                    TextBox ldata = new TextBox();
                    ldata.ID = "rdata_" + Convert.ToString(j) + "_" + Convert.ToString(i) + "_" + s;
                    ldata.Text = r[i] is null ? "" : Convert.ToString(r[i]);
                    ldata.Attributes.CssStyle.Add("color", "Black");
                    ldata.Attributes.CssStyle.Add("background-color", "White");
                    ldata.Attributes.CssStyle.Add("padding-left", "5px");
                    ldata.Attributes.CssStyle.Add("padding-right", "5px");
                    ldata.Enabled = false;
                    tcell.Controls.Add(ldata);
                    tcell.BorderWidth = new Unit(1);
                    tcell.BorderStyle = BorderStyle.Solid;
                    tcell.BorderColor = Color.Black;
                    tcell.BackColor = Color.White;
                    tcell.ForeColor = Color.Black;
                    i++;
                    AllRow.Cells.Add(tcell);
                    table.Rows.Add(AllRow);
                }
                SettingHolder.Controls.Add(table);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            GeneratePage();
        }

        protected void EditBtn_Click(object sender, EventArgs e)
        {
            var table= SettingHolder.FindControl("settingTable") as Table;
            var tablecontrols = table.Controls;
            foreach(TableRow c in tablecontrols)
            {
                var controlT = c.Cells[1].Controls[0];
                var idName = controlT.ID;
                if (idName.IndexOf("toplab") > -1)
                {
                    continue;
                }
                var textControl = controlT as TextBox;
                textControl.Enabled = true;
            }
            UpdateBtn.Enabled = true;
            UpdateBtn.Visible = true;
            CancelBtn.Enabled = true;
            CancelBtn.Visible = true;
            EditBtn.Visible = false;
            EditBtn.Enabled = false;
        }

        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            SettingHolder.Controls.Clear();
            GeneratePage();
            EditBtn.Enabled = true;
            EditBtn.Visible = true;
            UpdateBtn.Visible = false;
            UpdateBtn.Enabled = false;
            CancelBtn.Visible = false;
            CancelBtn.Enabled = false;            
        }

        private void UpdateListing(List<Listing> listings,string id,string field,string value)
        {
            int idnum = Convert.ToInt32(id);
            Listing L = listings.Find(x => x.Id == idnum);
            if (L is null)
            {
                L = new Listing();
                L.Id = idnum;
                switch (field)
                {
                    case "ListingId":
                        L.ListingId = value;
                        break;
                    case "PropertyAddress":
                        L.PropertyAddress = value;
                        break;
                    case "ICALURL":
                        L.IcalUrl = value;
                        break;
                }
                listings.Add(L);
            }
            else
            {
                foreach(Listing eachList in listings)
                {
                    if(L.Id==eachList.Id)
                    switch (field)
                    {
                        case "ListingId":
                                eachList.ListingId = value;
                            break;
                        case "PropertyAddress":
                                eachList.PropertyAddress = value;
                            break;
                        case "ICAL URL":
                                eachList.IcalUrl = value;
                            break;
                    }

                }
            }
        } 

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            Host H = new Host();
            List<Listing> listingsall = new List<Listing>();
            var table = SettingHolder.FindControl("settingTable") as Table;
            var tablecontrols = table.Controls;
            foreach (TableRow c in tablecontrols)
            {
                var controlT = c.Cells[1].Controls[0];
                var idName = controlT.ID;
                if (idName.IndexOf("toplab") > -1)
                {
                    continue;
                }
                var textControl = controlT as TextBox;
                if (idName.IndexOf("ldata")>-1)
                {
                    switch (textControl.ID.Split('_')[2])
                    {
                        case "FullName":
                            H.FullName = textControl.Text;
                            break;
                        case "FirstName":
                            H.FirstName = textControl.Text;
                            break;
                        case "LastName":
                            H.LastName = textControl.Text;
                            break;
                        case "Age":
                            H.Age = Convert.ToInt32(textControl.Text);
                            break;
                        case "Email":
                            H.Email = textControl.Text;
                            break;
                        case "Phone":
                            H.Phone = textControl.Text;
                            break;
                    }
                }
                else if (idName.IndexOf("rdata") > -1)
                {
                    string id = idName.Split('_')[1];
                    string field = idName.Split('_')[3];
                    string value = textControl.Text;
                    UpdateListing(listingsall,id,field,value);
                }
                textControl.Enabled = false;
            }
            H.Listings = listingsall;
            H.username = Convert.ToString(Session["UserName"]);
            Datalayer.UpdateSettings(H);
            foreach (TableRow c in tablecontrols)
            {
                var controlT = c.Cells[1].Controls[0];
                var idName = controlT.ID;
                if (idName.IndexOf("toplab") > -1)
                {
                    continue;
                }
                var textControl = controlT as TextBox;
                textControl.Enabled = false;
            }
            EditBtn.Enabled = true;
            EditBtn.Visible = true;
            CancelBtn.Visible = false;
            CancelBtn.Enabled = false;
            UpdateBtn.Visible = false;
            UpdateBtn.Enabled = false;
        }
    }
}
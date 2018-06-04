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

namespace WebAirplus
{
    public partial class _Default : Page
    {
        protected void StatusChange(object sender,EventArgs e)
        {
            //Label1.Text = sender.["ID"];
            var propertyInfo = sender.GetType().GetProperty("ID");
            var value = propertyInfo.GetValue(sender, null);

        }
        public delegate void StatusChangeHandler(object o, EventArgs e);
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
            Table table = new Table();
            //table.
            table.ID = "GuestTable";
            table.Width = Unit.Percentage(100);
            TableRow TopRow = new TableRow();
            string[] topvalues = { "FullName", "Check In", "Check Out", "Requested Check In", "Requested Check Out", "Status","Check Out Cleaning","Remarks"};
            int toplabelid=0;
            foreach (string s in topvalues)
            {
                TableCell c = new TableCell();
                Label l = new Label();

                l.ID = "toplab" + Convert.ToString(toplabelid++);
                l.Text = s;
                l.Attributes.CssStyle.Add("font-wight","bold");
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
                
                TopRow.Cells.Add(c);
            }
            table.Rows.Add(TopRow);
           
             
            int leavetoprow = 0;
            foreach(DataRow dr in data.Rows)
            {
                if (leavetoprow++ == 0)
                {
                    continue;
                }
                TableRow tr = new TableRow();
                tr.Attributes.CssStyle.Add("height", "100px");
                if (leavetoprow % 2 == 0)
                {
                    tr.Attributes.CssStyle.Add("background-color", "#FFFFE0");
                }
                else
                {
                    tr.Attributes.CssStyle.Add("background-color", "#FFE4B5	");
                }

                int[] labelsArray = { 3, 7,8};
                int[] calArray = { 5, 6, 10 };
                int[] dropdownArray = { 9 };
                int[] textArray = {11 };
                int[] sequence = {3,7,8,5,6,9,10,11 };
                for (int i = 0; i <sequence.Length; i++)
                {
                    int selitem=sequence[i];
                    TableCell c = new TableCell();
                    if (labelsArray.Contains(selitem))
                    {
                        Label l = new Label();
                        l.ID = "midrow" + Convert.ToString(leavetoprow) + Convert.ToString(i);
                        l.Text = Convert.ToString(dr[selitem]);
                        //l.Attributes.CssStyle.Add("color", "Black");
                        //l.Attributes.CssStyle.Add("padding-left", "5px");
                        //l.Attributes.CssStyle.Add("padding-right", "5px");
                        //c.BackColor = Color.LightYellow;
                        c.Controls.Add(l);
                    }
                    else if (textArray.Contains(selitem))
                    {
                        TextBox t = new TextBox();
                        t.ID= "text" + "_" + Convert.ToString(dr[0]) + "_" + Convert.ToString(dr[1]) + "_" + Convert.ToString(dr[2])+"_"+selitem;
                        GuestValues.textFields.Add(t.ID);
                        t.Text = Convert.ToString(dr[selitem]);
                        //t.Attributes.CssStyle.Add("color", "Black");
                        c.Controls.Add(t);
                    }
                    else if (calArray.Contains(selitem))
                    {
                        TextBox t = new TextBox();
                        t.ID = "cal" + "_" + Convert.ToString(dr[0]) + "_" + Convert.ToString(dr[1]) + "_" + Convert.ToString(dr[2]) + "_" + selitem;
                        GuestValues.textFields.Add(t.ID);
                        t.TextMode = TextBoxMode.DateTime;
                        if (Convert.ToString(dr[selitem]) == "")
                        {
                            t.Text = "00/00/0000 00:00:00";
                        }
                        else
                        {
                            t.Text = Convert.ToString(dr[selitem]);
                        }
                        //t.Attributes.CssStyle.Add("color", "Black");
                        c.Controls.Add(t);
                    }
                    else if(dropdownArray.Contains(selitem))
                    {
                        DropDownList statusdrop = new DropDownList();
                        statusdrop.DataTextField = "StatusValue";
                        statusdrop.DataValueField = "StatusCode_Id";
                        statusdrop.DataSource = statuscode_data;
                        statusdrop.ID = "dd" + "_" + Convert.ToString(dr[0]) + "_" + Convert.ToString(dr[1]) + "_" + Convert.ToString(dr[2]) + "_" + selitem;
                        GuestValues.textFields.Add(statusdrop.ID);
                        statusdrop.DataBind();
                        if (Convert.ToString(dr[selitem]) == "Empty")
                        {
                            statusdrop.SelectedIndex = 0;
                        }
                        else
                        {
                            foreach (DataRow d in statuscode_data.Rows)
                            {
                                if (Convert.ToString(d[1]) == Convert.ToString(dr[selitem]))
                                {
                                    statusdrop.SelectedValue = Convert.ToString(d[0]);
                                }
                            }
                        }
                        //statusdrop.SelectedIndexChanged += new EventHandler(StatusChange);
                        //statusdrop.AutoPostBack = true;
                        c.Controls.Add(statusdrop);
                    }
                    else
                    {

                    }
                    tr.Cells.Add(c);
                }
                table.Rows.Add(tr);                
            }
            GuestHolder.Controls.Add(table);

        }

        protected void btn_update_Click(object sender, EventArgs e)
        {

            int[] calArray = { 5, 6, 10 };
            int[] dropdownArray = { 9 };
            int[] textArray = { 11 };

            Button btn = sender as Button;
            var parenttab = btn.Parent;
            var tab = parenttab.FindControl("GuestTable");//
            DataTable dt = new DataTable("GUESTTYPETABLE");
            DataColumn column;
            DataRow[] rows;

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "GuestId";
            // Add the Column to the DataColumnCollection.
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "PropertyId";
            // Add the Column to the DataColumnCollection.
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "HostId";
            // Add the Column to the DataColumnCollection.
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.DateTime");
            column.ColumnName = "RequestedCheckIn";
            // Add the Column to the DataColumnCollection.
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.DateTime");
            column.ColumnName = "RequestedCheckOut";
            // Add the Column to the DataColumnCollection.
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.DateTime");
            column.ColumnName = "CheckOutCleaning";
            // Add the Column to the DataColumnCollection.
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "StatusCode";
            // Add the Column to the DataColumnCollection.
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Remarks";
            // Add the Column to the DataColumnCollection.
            dt.Columns.Add(column);
            Dictionary<string,GuestInsert> guestlist = new Dictionary<string, GuestInsert>();
            foreach (string s in GuestValues.textFields)
            {
                var textcontrol = tab.FindControl(s);
                TextBox t;
                DropDownList ddl;
                string index="";
                GuestInsert guest = new GuestInsert();
                if (textcontrol is null)
                {

                }
                else
                {
                    if (textcontrol.ID.IndexOf("dd") != 0)
                    {
                        t = textcontrol as TextBox;
                        string pkvalue = textcontrol.ID;
                        string[] pkarray = pkvalue.Split('_');
                        int lastIndex = pkarray.Length - 1;
                        if (pkarray[0] == "text")
                        {
                            guest.GuestId = Convert.ToInt32(pkarray[1]);
                            guest.PropertyId = Convert.ToInt32(pkarray[2]);
                            guest.HostId = Convert.ToInt32(pkarray[3]);
                            index = "row_" + pkarray[1] + "_" + pkarray[2] + "_" + pkarray[3];
                            if (index != "")
                            {
                                GuestInsert g;
                                guestlist.TryGetValue(index, out g);
                                if (g is null)
                                {
                                    switch (pkarray[lastIndex])
                                    {
                                        case "11":
                                            guest.Remarks = t.Text;
                                            break;
                                    }
                                    guestlist.Add(index, guest);
                                }
                                else
                                {
                                    switch (pkarray[lastIndex])
                                    {
                                        case "11":
                                            g.Remarks = t.Text;
                                            break;
                                    }
                                    guestlist[index] = g;
                                }
                            }

                        }
                        else if (pkarray[0] == "cal")
                        {
                            guest.GuestId = Convert.ToInt32(pkarray[1]);
                            guest.PropertyId = Convert.ToInt32(pkarray[2]);
                            guest.HostId = Convert.ToInt32(pkarray[3]);
                            index = "row_" + pkarray[1] + "_" + pkarray[2] + "_" + pkarray[3];
                            string date = t.Text;
                            DateTime dDate = new DateTime();
                            if (date != "")
                            {
                                if (date.IndexOf("/")!=-1)
                                {
                                    string[] dateArray = date.Split('/');
                                    if (dateArray.Length > 2)
                                    {
                                        string month = dateArray[0];
                                        string day = dateArray[1];
                                        string Year = dateArray[2];
                                        //if (String.IsNullOrEmpty(dateArray[3]))
                                        //{

                                        //}
                                        if (Convert.ToInt32(month) == 0 || Convert.ToInt32(day) == 0)
                                        {
                                            
                                        }
                                        else
                                        {
                                            dDate = Convert.ToDateTime(date);
                                        }

                                    }
                                }
                                else
                                {
                                    System.Web.HttpContext.Current.Response.Write(@"<SCRIPT LANGUAGE=""JavaScript"">alert('Please check the date format')</SCRIPT>");
                                    t.Focus();
                                    return;
                                }
                                if (index != "")
                                {
                                    GuestInsert g;
                                    guestlist.TryGetValue(index, out g);
                                    if (g is null)
                                    {
                                        switch (pkarray[lastIndex])
                                        {
                                            case "5":

                                                if (Convert.ToInt32(dDate.Year) == 1)
                                                {
                                                    guest.RequestedCheckIn = null;
                                                    break;
                                                }
                                                guest.RequestedCheckIn = dDate;
                                                break;
                                            case "6":
                                                if (Convert.ToInt32(dDate.Year) == 1)
                                                {
                                                    guest.RequestedCheckOut = null;
                                                    break;
                                                }
                                                guest.RequestedCheckOut = dDate;
                                                break;
                                            case "10":
                                                if (Convert.ToInt32(dDate.Year) == 1)
                                                {
                                                    guest.CheckOutCleaning = null;
                                                    break;
                                                }
                                                guest.CheckOutCleaning = dDate;
                                                break;
                                        }
                                        guestlist.Add(index, guest);
                                    }
                                    else
                                    {
                                        switch (pkarray[lastIndex])
                                        {
                                            case "5":

                                                if (Convert.ToInt32(dDate.Year) == 1)
                                                {
                                                    g.RequestedCheckIn = null;
                                                    break;
                                                }
                                                g.RequestedCheckIn = dDate;
                                                break;
                                            case "6":
                                                if (Convert.ToInt32(dDate.Year) == 1)
                                                {
                                                    g.RequestedCheckOut = null;
                                                    break;
                                                }
                                                g.RequestedCheckOut = dDate;
                                                break;
                                            case "10":
                                                if (Convert.ToInt32(dDate.Year) == 1)
                                                {
                                                    g.CheckOutCleaning = null;
                                                    break;
                                                }
                                                g.CheckOutCleaning = dDate;
                                                break;
                                        }
                                        guestlist[index] = g;
                                    }
                                }

                            }


                        }
                    }
                    else
                    {
                        ddl = textcontrol as DropDownList;
                        string pkvalue = ddl.ID;
                        string[] pkarray = pkvalue.Split('_');
                        int lastIndex = pkarray.Length - 1;
                        guest.GuestId = Convert.ToInt32(pkarray[1]);
                        guest.PropertyId = Convert.ToInt32(pkarray[2]);
                        guest.HostId = Convert.ToInt32(pkarray[3]);
                        index = "row_" + pkarray[1] + "_" + pkarray[2] + "_" + pkarray[3];
                        int val = Convert.ToInt32(ddl.SelectedItem.Value);
                        if (index != "")
                        {
                            GuestInsert g;
                            guestlist.TryGetValue(index, out g);
                            if (g is null)
                            {
                                guest.status = val;
                                guestlist.Add(index, guest);
                            }
                            else
                            {
                                g.status = val;
                                guestlist[index] = g;
                            }
                        }
                        //guest.status = val;

                    }

                }
            }
            foreach(GuestInsert gGuest in guestlist.Values)
            {
                DataRow row = dt.NewRow();
                row["GuestId"] = gGuest.GuestId;
                row["PropertyId"] = gGuest.PropertyId;
                row["HostId"] = gGuest.HostId;
                if (gGuest.RequestedCheckIn == null)
                {
                    row["RequestedCheckIn"] = DBNull.Value;
                }
                else
                {
                    row["RequestedCheckIn"] = gGuest.RequestedCheckIn;
                }
                if (gGuest.RequestedCheckOut == null)
                {
                    row["RequestedCheckOut"] = DBNull.Value;
                }
                else
                {
                    row["RequestedCheckOut"] = gGuest.RequestedCheckOut;
                }

                if (gGuest.CheckOutCleaning == null)
                {
                    row["CheckOutCleaning"] = DBNull.Value;
                }
                else
                {
                    row["CheckOutCleaning"] = gGuest.CheckOutCleaning;
                }
                if (gGuest.status == 0)
                {
                    row["StatusCode"] = DBNull.Value;
                }
                else
                {
                    row["StatusCode"] = gGuest.status;
                }
                row["Remarks"] = gGuest.Remarks;
                
                dt.Rows.Add(row);
            }
            Datalayer.UpdateGuestProperty(dt);
        }
    }
}
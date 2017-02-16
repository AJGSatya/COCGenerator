using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;

public partial class ConnectionEncryption : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ArrayList list = new ArrayList(16);
        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

        // Create "Root Sections" entry
        ConfigSectionData csd1 = new ConfigSectionData();
        csd1.Name = "__root";
        list.Add(csd1);

        // Create <appSettings> entry
        ConfigurationSection section = config.GetSection("appSettings");
        list.Add(GetConfigSectionData(section));

        // Create <connectionStrings> entry
        section = config.GetSection("connectionStrings");
        list.Add(GetConfigSectionData(section));

        // Create "Root Sections" entry
        ConfigSectionData csd2 = new ConfigSectionData();
        csd2.Name = "__system";
        list.Add(csd2);

        // Create <system.web> entries
        //ConfigurationSectionGroup group = config.SectionGroups["system.web"];
        //for (int i = 0; i < group.Sections.Count; i++)
        //{
        //    section = group.Sections[i];
        //    if (!section.SectionInformation.IsLocked)
        //        list.Add(GetConfigSectionData(section));
        //}
        
        // Display the results
        GridView1.DataSource = list;
        GridView1.DataBind();
    }

    ConfigSectionData GetConfigSectionData(ConfigurationSection section)
    {
        ConfigSectionData csd = new ConfigSectionData();
        csd.Name = section.SectionInformation.Name;
        csd.IsDeclared = section.SectionInformation.IsDeclared;
        csd.IsProtected = section.SectionInformation.IsProtected;

        switch (section.SectionInformation.AllowDefinition)
        {
            case ConfigurationAllowDefinition.MachineOnly:
                csd.Scope = "Machine";
                break;

            case ConfigurationAllowDefinition.MachineToApplication:
                csd.Scope = "Machine/Application";
                break;

            case ConfigurationAllowDefinition.Everywhere:
                csd.Scope = "Anywhere";
                break;

            default:
                csd.Scope = "Unknown";
                break;
        }
        return csd;
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Look for special "marker" entries ("__root" and "__system") and
        // handle those by building embedded header rows
        GridViewRow row = e.Row;
        if (row.Cells[0].Text == "__root")
            CreateEmbeddedHeaderRow(row, "Root Sections");
        else if (row.Cells[0].Text == "__system")
            CreateEmbeddedHeaderRow(row, "&lt;system.web&gt; Sections");

    }
    protected void CreateEmbeddedHeaderRow(GridViewRow row, string title)
    {
        int count = row.Cells.Count;
        for (int i = 0; i < count - 1; i++)
            row.Cells.RemoveAt(1);
        row.Cells[0].Text = title;
        row.Cells[0].ColumnSpan = 5;
        row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        row.BackColor = System.Drawing.Color.LemonChiffon;
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string command = e.CommandName;
        string path = e.CommandArgument.ToString();

        if (path != "appSettings" && path != "connectionStrings")
            path = "system.web/" + path;

        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        ConfigurationSection section = config.GetSection(path);

        if (command == "Encrypt")
        {
            section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            config.Save();
        }
        else if (command == "Decrypt")
        {
            section.SectionInformation.UnprotectSection();
            config.Save();
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.ApplicationPath);
    }
}
class ConfigSectionData
{
    string _Name;
    public string Name
    {
        get
        {
            return _Name;
        }

        set
        {
            _Name = value;
        }
    }

    bool _IsDeclared;
    public bool IsDeclared
    {
        get
        {
            return _IsDeclared;
        }

        set
        {
            _IsDeclared = value;
        }
    }

    bool _IsProtected;
    public bool IsProtected
    {
        get
        {
            return _IsProtected;
        }

        set
        {
            _IsProtected = value;
        }
    }

    string _Scope;
    public string Scope
    {
        get
        {
            return _Scope;
        }

        set
        {
            _Scope = value;
        }
    }
}

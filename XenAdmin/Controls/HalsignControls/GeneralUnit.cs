using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalsignLib;
using XenAPI;
using XenAdmin.Core;
using XenAdmin.Network;
using XenAdmin.CustomFields;
using XenAdmin.Model;
using XenAdmin.Utils;
using XenAdmin.Commands;
using XenAdmin.Properties;
using XenAdmin.Dialogs;
using XenAdmin.SettingsPanels;

namespace XenAdmin.Controls
{
    public class GeneralDataStructure
    {
        public GeneralDataStructure(string key, string value, string group)
        {
            this._key = string.Format("{0}:", key);
            this._value = value;
            this._group = group;
        }

        public GeneralDataStructure(string key, string value, string group, Color color)
            : this(key, value, group)
        {
            this._color = color;
        }

        public GeneralDataStructure(string key, string value, string group, List<ToolStripMenuItem> collection)
            : this(key, value, group)
        {
            this._contextMenuCollection = collection;
        }

        public GeneralDataStructure(string key, string value, string group, Color color, List<ToolStripMenuItem> collection)
            : this(key, value, group, color)
        {
            this._contextMenuCollection = collection;
        }

        public GeneralDataStructure(string key, string value, string group, List<ToolStripMenuItem> collection, bool isLink)
            : this(key, value, group, collection)
        {
            this._isLink = isLink;
        }

        private string _key;
        public string _Key
        {
            get { return this._key; }
            private set { this._key = value; }
        }

        private string _value;
        public string _Value
        {
            get { return this._value; }
            private set { this._value = value; }
        }

        private string _group;
        public string _Group
        {
            get { return _group; }
            private set { this._group = value; }
        }

        private bool _isLink;
        public  bool _IsLink
        {
            get { return this._isLink; }
            set { this._isLink = value; }
        }

        private Color _color;
        public Color _Color
        {
            get { return this._color; }
            set { this._color = value; }
        }

        private List<ToolStripMenuItem> _contextMenuCollection;
        public List<ToolStripMenuItem> _ContextMenuCollection
        {
            get { return this._contextMenuCollection; }
            set { this._contextMenuCollection = value; }
        }
    }
}

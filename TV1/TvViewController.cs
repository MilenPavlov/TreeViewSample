using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Newtonsoft.Json.Linq;
using UIKit;

namespace TV1
{
    public partial class TvViewController : UIViewController
    {
        
        public TvViewController(IntPtr handle) : base(handle)
        {           
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.Source = new TreeViewSource(TableView, ScrollView);
        }
    }


    public class TreeViewSource : UITableViewSource
    {
        //const string Input = @"{'title': 'Root Folder', 'type':'folder','filepath':'', 'children' :[{'title': 'Subfolder1','type':'folder','filepath':'', 'children' :[{'title': 'Subsubfolder','type':'folder','filepath':'', 'children' :[{'title': 'This file name is very ... very long .... !!!', 'type':'file','filepath':'','children' :[]}]},{'title': 'Empty','type':'folder', 'filepath':'','children' :[]}]},{'title': 'Subfolder2','type':'folder','filepath':'', 'children' :[]}]}";
        private readonly UITableView _tableView;
        private readonly UIScrollView _scrollView;
        public List<TreeNode> Nodes;


        public TreeViewSource(UITableView tableView,UIScrollView scrollView)
        {
            _tableView = tableView;
            _scrollView = scrollView;

            Nodes = new List<TreeNode>();

            var containerParrent = SetUpData();

            Nodes.Add(containerParrent);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var node = Nodes[indexPath.Row];
            var cell = tableView.DequeueReusableCell("TreeViewCell") as TreeViewCell;
            cell?.SetCellContents(node);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Nodes.Count;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 25;
        }      

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);

            var node = Nodes[indexPath.Row];
            var children = new List<TreeNode>();
            node.IsExpanded = true;
            if (node.Children.Any())
            {
                children.AddRange(node.Children);

                var isInserted = false;

                foreach (int index in children.Select(child => Nodes.IndexOf(child)))
                {
                    isInserted = (index > 0 && index != int.MaxValue);
                    if (isInserted)
                    {
                        break;
                    }
                }

                var arrayCells = new List<NSIndexPath>();
                if (isInserted)
                {
                    ColapseRow(tableView, children);
                    tableView.ReloadData();
                }
                else
                {
                    var count = indexPath.Row + 1;

                    foreach (var treeNode in children)
                    {
                        treeNode.IsExpanded = false;
                        //if there's  Parent's children not selected remove them
                        var nodesToRemove = new List<TreeNode>();

                        if (treeNode.Parent != null)
                        {
                            if (treeNode.Parent.Children.Any() && treeNode.Parent.Parent != null)
                            {
                                nodesToRemove.AddRange(treeNode.Parent.Parent.Children.Where(x=>!x.IsExpanded));

                                if (nodesToRemove.Any())
                                {
                                    ColapseRow(tableView, nodesToRemove);
                                }
                            }
                        }
                        
                        arrayCells.Add(NSIndexPath.FromRowSection(count, 0));
                        Nodes.Insert(count++, treeNode);
                    }

                    tableView.InsertRows(arrayCells.ToArray(), UITableViewRowAnimation.None);

                    tableView.ReloadData();
                }
            }
        }

        private void ColapseRow(UITableView tableView, List<TreeNode> children)
        {           
            foreach (var child in children.Distinct())
            {
                int indexToRemove = Nodes.IndexOf(child);

                if (child.Children != null && child.Children.Any())
                {
                    ColapseRow(tableView, child.Children);
                }

                if (Nodes.Contains(child))
                {
                    Nodes.Remove(child);
                    NSIndexPath[] indexArray = {NSIndexPath.FromRowSection(indexToRemove, 0)};
                    tableView.DeleteRows(indexArray, UITableViewRowAnimation.None);
                }

                child.IsExpanded = false;
            }           
        }

        private TreeNode ConvertJsonInput(TreeNode parent, JObject input, int depth)
        {
            var node = new TreeNode
            {
                NodeName = input["title"].ToString(),
                NodeType = input["type"].ToString(),
                FilePath = input["filepath"].ToString(),
                NodeLevel = depth + 1,
                Parent = parent
            };

            foreach (var c in input["children"])
            {
                node.Children.Add(ConvertJsonInput(node, JObject.Parse(c.ToString()), node.NodeLevel));
            }

            return node;
        }

        private TreeNode SetUpData()
        {
            var containerParrent = new TreeNode { NodeLevel = 1, NodeName = "Root Folder", NodeType = "folder" };

            var nodeLevel21 = new TreeNode { NodeName = "Subfolder 1", NodeLevel = 2, Parent = containerParrent, NodeType = "folder" };
            var nodeLevel22 = new TreeNode { NodeName = "Subfolder 2", NodeLevel = 2, Parent = containerParrent, NodeType = "folder" };

            var nodeLevel31 = new TreeNode { NodeName = "Subsubfolder", NodeLevel = 3, NodeType = "folder", Parent = nodeLevel21 };
            var nodeLevel32 = new TreeNode() { NodeName = "Empty", NodeLevel = 3, NodeType = "folder", Parent = nodeLevel21 };

            var nodeLevel4 = new TreeNode() { NodeName = "I'm a file!!", NodeLevel = 4, NodeType = "file", Parent = nodeLevel31 };

            containerParrent.Children.Add(nodeLevel21);
            containerParrent.Children.Add(nodeLevel22);

            nodeLevel21.Children.Add(nodeLevel31);
            nodeLevel21.Children.Add(nodeLevel32);

            nodeLevel31.Children.Add(nodeLevel4);

            return containerParrent;
        }
    }

    public class TreeNode
    {
        public string NodeName { get; set; }
        public string NodeType { get; set; }
        public int NodeLevel { get; set; }
        public string FilePath { get; set; }
        public List<TreeNode> Children { get; set; }
        public TreeNode Parent { get; set; }
        public bool IsExpanded { get; set; }
        public TreeNode()
        {
            Children = new List<TreeNode>();
        }
    }
}
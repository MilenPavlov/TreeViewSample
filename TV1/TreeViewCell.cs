using System;
using System.Drawing;
using UIKit;

namespace TV1
{
	partial class TreeViewCell : UITableViewCell
    {
        public int Level;

        private const int LevelIndent = 25;
        private const int CellHeight = 25;
       
        private UIImageView _imageView;
	    private UILabel _titleLabel;

        public TreeViewCell (IntPtr handle) : base (handle)
		{
		}       

        public TreeViewCell(UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
        {
        }

        public void SetCellContents(TreeNode node)
        {
            Level = node.NodeLevel;

            _imageView = new UIImageView
            {
                Image = (node.NodeType == "folder") ? UIImage.FromFile("folder.png") : UIImage.FromFile("file.png"),
                ContentMode = UIViewContentMode.Left
            };
            _imageView.SizeToFit();
            //_imageView.Frame = new RectangleF((float)_imageView.Frame.X, (float)_imageView.Frame.Y, (float)_imageView.Bounds.Width, (float)_imageView.Bounds.Height);

            _titleLabel = new UILabel()
            {
                TextColor = UIColor.Black,
                BackgroundColor = UIColor.Clear,
                Text = node.NodeName
            };
            _titleLabel.SizeToFit();
            //_titleLabel.Frame = new RectangleF((float)_titleLabel.Frame.X, (float)_titleLabel.Frame.Y, (float)_titleLabel.Bounds.Width, (float)_titleLabel.Bounds.Height);

            foreach (var subview in ContentView.Subviews)
            {
                subview.RemoveFromSuperview();
            }

            ContentView.AddSubviews(_imageView, _titleLabel);        
        }

	    public override void LayoutSubviews()
	    {
	        base.LayoutSubviews();

	        var contentRect = ContentView.Bounds;
	        var boundsX = contentRect.X;   

	        var imageFrame = new RectangleF(((float)boundsX + Level - 1) * LevelIndent, 0, (LevelIndent), CellHeight);
            _imageView.Frame = imageFrame;

            var titleFrame = new RectangleF(((float)boundsX + Level - 1) * LevelIndent + (LevelIndent) + 2, 0, (float)_titleLabel.Bounds.Width, CellHeight);
            _titleLabel.Frame = titleFrame;
        }
    }
}

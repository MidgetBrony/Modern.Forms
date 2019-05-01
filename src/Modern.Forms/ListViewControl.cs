﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Modern.Forms
{
    public class ListViewControl : ModernControl
    {
        public new static ControlStyle DefaultStyle = new ControlStyle (ModernControl.DefaultStyle, 
            (style) => style.BackgroundColor = Theme.FormBackgroundColor);

        public override ControlStyle Style { get; } = new ControlStyle (DefaultStyle);

        public List<ListViewControlItem> Items { get; } = new List<ListViewControlItem> ();

        public event EventHandler<EventArgs<ListViewControlItem>> ItemDoubleClicked;

        protected override Size DefaultSize => new Size (450, 450);

        protected override void OnPaint (SKPaintEventArgs e)
        {
            base.OnPaint (e);

            LayoutItems ();

            foreach (var item in Items)
                item.DrawItem (e.Canvas);
        }

        protected override void OnMouseClick (MouseEventArgs e)
        {
            base.OnMouseClick (e);

            var clicked_item = Items.FirstOrDefault (tp => tp.Bounds.Contains (e.Location));

            SetSelectedItem (clicked_item);
        }

        protected override void OnMouseDoubleClick (MouseEventArgs e)
        {
            base.OnMouseDoubleClick (e);

            var clicked_item = Items.FirstOrDefault (tp => tp.Bounds.Contains (e.Location));

            if (clicked_item != null)
                ItemDoubleClicked?.Invoke (this, new EventArgs<ListViewControlItem> (clicked_item));
        }

        public void SetSelectedItem (ListViewControlItem item)
        {
            var old = Items.FirstOrDefault (i => i.Selected);

            if (old == item)
                return;

            if (old != null)
                old.Selected = false;

            if (item != null)
                item.Selected = true;

            Invalidate ();
        }

        private void LayoutItems ()
        {
            var x = 3;
            var y = 3;
            var item_width = 70;
            var item_height = 70;
            var item_padding = 6;

            foreach (var item in Items) {
                item.SetBounds (x, y, item_width, item_height);
                x += item_width + item_padding;

                if (x + item_width > Width) {
                    x = 3;
                    y += item_height + item_padding;
                }
            }
        }
    }
}

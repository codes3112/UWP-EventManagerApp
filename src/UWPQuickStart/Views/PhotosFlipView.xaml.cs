// Copyright (c) Microsoft. All rights reserved
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using UWPQuickStart.Models;
using Windows.UI.Xaml.Controls;

namespace UWPQuickStart.Views
{
    public sealed partial class PhotosFlipView : UserControl
    {
        public PhotosFlipView()
        {
            InitializeComponent();
        }
        private void FlipView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var model = DataContext as PhotoStreamModel;
            if (model != null)
            {
                model.SelectedItem = e.ClickedItem as PhotoModel;
                model.ViewSelectionMode = ViewSelectionMode.Flip;
            }
        }
    }
}
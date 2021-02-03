using System.Windows;
using System.Windows.Controls;
using Emma.XamlControls.SelectableTextBlock;

namespace Emma.XamlControls.Controls
{
    // FROM: https://stackoverflow.com/questions/136435/any-way-to-make-a-wpf-textblock-selectable

    public class SelectableTextBlock : TextBlock
    {
        static SelectableTextBlock()
        {
            FocusableProperty.OverrideMetadata(typeof(SelectableTextBlock), new FrameworkPropertyMetadata(true));
            TextEditorWrapper.RegisterCommandHandlers(typeof(SelectableTextBlock), true, true, true);

            // remove the focus rectangle around the control
            FocusVisualStyleProperty.OverrideMetadata(typeof(SelectableTextBlock), new FrameworkPropertyMetadata((object)null));
        }

        private readonly TextEditorWrapper _editor;

        public SelectableTextBlock()
        {
            _editor = TextEditorWrapper.CreateFor(this);
        }
    }
}
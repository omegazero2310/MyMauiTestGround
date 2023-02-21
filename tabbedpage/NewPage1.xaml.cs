using Bertuzzi.MAUI.EventAggregator;

namespace tabbedpage;

public partial class NewPage1 : ContentPage
{
    bool ishidden;
	public NewPage1()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        ishidden = !ishidden;
        EventAggregator.Instance.SendMessage(ishidden);
    }
}
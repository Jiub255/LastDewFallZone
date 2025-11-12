using Godot;

namespace Lastdew
{	
	public partial class TimerTester : Node
	{
		private Lastdew.Timer _timer;
	
		public override void _Ready()
		{
			base._Ready();
	
			_timer = new Lastdew.Timer(TestMethod, 0.5f, true);
		}
	
		public override void _Process(double delta)
		{
			base._Process(delta);
			
			_timer.Tick(delta);
		}
		
		private void TestMethod()
		{
			GD.Print("Timer test success");
		}
	}
}

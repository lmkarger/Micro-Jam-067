using Godot;
using System;

public partial class CelestialCycleDisplay : Control
{
    private const float rotationSmoothing = 10f;

    [Export]
    private Color _dayColor;

    [Export]
    private Color _nightColor;

    [Export]
    private TextureRect _partitionTextureRect;

    [Export]
    private TextureRect _celestialTextureRect;

    private float cycleAngle = 0f;
    private Gradient _partitionGradient;
    private GradientTexture2D _partitionGradientTexture;

    private Gradient _celestialGradient;
    private GradientTexture2D _celestialGradientTexture;

    public override void _Ready()
    {
        base._Ready();

        GameManager.Instance.GameBegan += Initialize;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameBegan -= Initialize;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!IsInstanceValid(_partitionGradientTexture)) return;

        float cycleLength = (float)GameManager.Instance.NightDuration + GameManager.Instance.DayDuration;
        float targetAngle = (float)Math.Tau * ((float)(GameManager.Instance.GameTime % cycleLength) + 0.5f) / cycleLength;
        float step = 1f - (float)Mathf.Exp(-delta * rotationSmoothing);


        cycleAngle = Mathf.LerpAngle(cycleAngle, targetAngle, step);

        _partitionGradientTexture.FillTo = Vector2.Up.Rotated(cycleAngle) + Vector2.One / 2f;
        _celestialGradientTexture.FillTo = Vector2.Up.Rotated(cycleAngle) + Vector2.One / 2f;
    }

    private void Initialize()
    {
        CreatePartitionGradient();
        _partitionTextureRect.Texture = _partitionGradientTexture;

        CreateCelestialGradient();
        _celestialTextureRect.Texture = _celestialGradientTexture;
    }

    private void CreateCelestialGradient()
    {
        _celestialGradient = new();
        _celestialGradient.InterpolationMode = Gradient.InterpolationModeEnum.Constant;

        int cycleLength = GameManager.Instance.NightDuration + GameManager.Instance.DayDuration;


        for (int i = 0; i < cycleLength; i++)
        {
            float progress = (float)(cycleLength - i - 1) / (float)(cycleLength);

            Color color = i < GameManager.Instance.NightDuration ? _nightColor : _dayColor;
            _celestialGradient.AddPoint(progress, color);
        }

        _celestialGradientTexture = new();
        _celestialGradientTexture.Fill = GradientTexture2D.FillEnum.Conic;
        _celestialGradientTexture.Gradient = _celestialGradient;
        _celestialGradientTexture.FillFrom = Vector2.One / 2f;
        _celestialGradientTexture.FillTo = Vector2.Right + Vector2.One / 2f;
    }

    private void CreatePartitionGradient()
    {
        _partitionGradient = new();
        _partitionGradient.InterpolationMode = Gradient.InterpolationModeEnum.Constant;

        int cycleLength = GameManager.Instance.NightDuration + GameManager.Instance.DayDuration;
        int numPartitions = LeastPrimeFactor(cycleLength);

        for (int i = 0; i < cycleLength; i++)
        {
            float progress = (float)(i) / (float)(cycleLength);
            float colorProgress = (float)(i % numPartitions) / (float)(numPartitions - 1);

            Color partitionColor = new Color(colorProgress, colorProgress, colorProgress, 1);
            _partitionGradient.AddPoint(progress, partitionColor);
        }

        _partitionGradientTexture = new();
        _partitionGradientTexture.Fill = GradientTexture2D.FillEnum.Conic;
        _partitionGradientTexture.Gradient = _partitionGradient;
        _partitionGradientTexture.FillFrom = Vector2.One / 2f;
        _partitionGradientTexture.FillTo = Vector2.Right + Vector2.One / 2f;
    }

    private static int LeastPrimeFactor(int n)
    {
        if (n < 2) return n;

        for (int i = 2; i <= Math.Sqrt((double)(n)); i++)
        {
            if (n % i == 0) return i;
        }

        return n;
    }
}

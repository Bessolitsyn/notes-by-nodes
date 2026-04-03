using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using GraphControl.Model;
using Windows.Foundation;

namespace GraphControl
{
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    public class GraphControl : Control
    {
        private Canvas? _canvasPart;
        private ScrollViewer? _scrollViewerPart;

        private readonly Dictionary<IGraphControlNode, Ellipse> _nodeEllipses = new();
        private Point _dragStart;
        private Ellipse? _draggedEllipse;
        private const double _canvasPadding = 20.0;

        private ObservableCollection<IGraphControlNode> _nodes = new();
        public ObservableCollection<IGraphControlNode> Nodes
        {
            get => _nodes;
            set
            {
                if (_nodes == value) return;
                if (_nodes != null) _nodes.CollectionChanged -= Nodes_CollectionChanged;
                _nodes = value ?? new ObservableCollection<IGraphControlNode>();
                _nodes.CollectionChanged += Nodes_CollectionChanged;
                DrawGraph();
            }
        }

        public GraphControl()
        {
            this.DefaultStyleKey = typeof(GraphControl);
            this.Loaded += GraphControl_Loaded;
        }

        private void GraphControl_Loaded(object sender, RoutedEventArgs e)
        {
            DrawGraph();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvasPart = GetTemplateChild("PART_Canvas") as Canvas;
            _scrollViewerPart = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;

            if (_canvasPart != null)
            {
                _canvasPart.SizeChanged += (s, e) => UpdateCanvasSize();
            }

            DrawGraph();
        }

        public void LoadFrom(IGraphControlNode root)
        {
            if (root?.ChildNodes != null)
            {
                Nodes = root.ChildNodes;
            }
            else
            {
                Nodes = new ObservableCollection<IGraphControlNode>();
            }
            DrawGraph();
        }

        private void Nodes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            DispatcherQueue.TryEnqueue(() => DrawGraph());
        }

        private void DrawGraph()
        {
            if (_canvasPart == null) return;

            _canvasPart.Children.Clear();
            _nodeEllipses.Clear();

            double x = 50, y = 50, dx = 120, dy = 120;
            foreach (var node in Nodes)
            {
                var ellipse = CreateNodeEllipse(node, x, y);
                _nodeEllipses[node] = ellipse;
                _canvasPart.Children.Add(ellipse);
                x += dx;
                y += dy;
            }

            // Draw edges after nodes are placed
            foreach (var kvp in _nodeEllipses)
            {
                var parentNode = kvp.Key;
                var parentEllipse = kvp.Value;

                foreach (var child in parentNode.ChildNodes)
                {
                    if (_nodeEllipses.TryGetValue(child, out var childEllipse))
                    {
                        DrawEdge(parentEllipse, childEllipse);
                    }
                }
            }

            UpdateCanvasSize();
        }

        private Ellipse CreateNodeEllipse(IGraphControlNode node, double x, double y)
        {
            var ellipse = new Ellipse
            {
                Width = 60,
                Height = 60,
                Fill = new SolidColorBrush(Microsoft.UI.Colors.LightBlue),
                Stroke = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue),
                StrokeThickness = 2,
                Tag = node
            };

            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);

            ellipse.PointerPressed += Ellipse_PointerPressed;
            ellipse.PointerMoved += Ellipse_PointerMoved;
            ellipse.PointerReleased += Ellipse_PointerReleased;

            return ellipse;
        }

        private void DrawEdge(Ellipse parent, Ellipse child)
        {
            if (_canvasPart == null) return;

            var line = new Line
            {
                Stroke = new SolidColorBrush(Microsoft.UI.Colors.Gray),
                StrokeThickness = 2,
                X1 = Canvas.GetLeft(parent) + parent.Width / 2,
                Y1 = Canvas.GetTop(parent) + parent.Height / 2,
                X2 = Canvas.GetLeft(child) + child.Width / 2,
                Y2 = Canvas.GetTop(child) + child.Height / 2
            };
            // Insert lines before ellipses so lines render under nodes
            _canvasPart.Children.Insert(0, line);
        }

        private void Ellipse_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _draggedEllipse = sender as Ellipse;
            if (_draggedEllipse == null || _canvasPart == null) return;

            _dragStart = e.GetCurrentPoint(_canvasPart).Position;
            _draggedEllipse.CapturePointer(e.Pointer);
            e.Handled = true;
        }

        private void Ellipse_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_draggedEllipse != null && _canvasPart != null)
            {
                var point = e.GetCurrentPoint(_canvasPart);
                if (point.Properties.IsLeftButtonPressed)
                {
                    var pos = point.Position;
                    double dx = pos.X - _dragStart.X;
                    double dy = pos.Y - _dragStart.Y;

                    double left = Canvas.GetLeft(_draggedEllipse) + dx;
                    double top = Canvas.GetTop(_draggedEllipse) + dy;

                    Canvas.SetLeft(_draggedEllipse, left);
                    Canvas.SetTop(_draggedEllipse, top);

                    _dragStart = pos;
                    RedrawEdges();
                    UpdateCanvasSize();
                    e.Handled = true;
                }
            }
        }

        private void Ellipse_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (_draggedEllipse != null)
            {
                _draggedEllipse.ReleasePointerCapture(e.Pointer);
                _draggedEllipse = null;
                e.Handled = true;
            }
        }

        private void RedrawEdges()
        {
            if (_canvasPart == null) return;

            for (int i = _canvasPart.Children.Count - 1; i >= 0; i--)
            {
                if (_canvasPart.Children[i] is Line)
                    _canvasPart.Children.RemoveAt(i);
            }

            foreach (var kvp in _nodeEllipses)
            {
                var parentNode = kvp.Key;
                var parentEllipse = kvp.Value;

                foreach (var child in parentNode.ChildNodes)
                {
                    if (_nodeEllipses.TryGetValue(child, out var childEllipse))
                    {
                        DrawEdge(parentEllipse, childEllipse);
                    }
                }
            }
        }

        private void UpdateCanvasSize()
        {
            if (_canvasPart == null) return;

            double maxRight = 0;
            double maxBottom = 0;

            foreach (var ellipse in _nodeEllipses.Values)
            {
                double left = Canvas.GetLeft(ellipse);
                double top = Canvas.GetTop(ellipse);
                double right = left + ellipse.Width;
                double bottom = top + ellipse.Height;

                if (right > maxRight) maxRight = right;
                if (bottom > maxBottom) maxBottom = bottom;
            }

            double contentWidth = maxRight + _canvasPadding;
            double contentHeight = maxBottom + _canvasPadding;

            double minWidth = ActualWidth;
            double minHeight = ActualHeight;

            if (_scrollViewerPart != null)
            {
                if (_scrollViewerPart.ViewportWidth > 0) minWidth = _scrollViewerPart.ViewportWidth;
                if (_scrollViewerPart.ViewportHeight > 0) minHeight = _scrollViewerPart.ViewportHeight;
            }

            _canvasPart.Width = Math.Max(contentWidth, minWidth);
            _canvasPart.Height = Math.Max(contentHeight, minHeight);
        }

        public Canvas? CanvasPart => _canvasPart;
    }
}

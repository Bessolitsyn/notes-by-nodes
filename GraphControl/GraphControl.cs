using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphControl.Model;

namespace notes_by_nodes.GraphControl
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GraphControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GraphControl;assembly=GraphControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    /// 
    /// This control now implements the NoteGraphControl behavior using template parts:
    /// - PART_ScrollViewer (ScrollViewer) and PART_Canvas (Canvas).
    /// </summary>
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
                //DrawGraph();
            }
        }

        static GraphControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphControl), new FrameworkPropertyMetadata(typeof(GraphControl)));        
        }

        public GraphControl()
        {
            Loaded += GraphControl_Loaded;
        }

        private void GraphControl_Loaded(object sender, RoutedEventArgs e)
        {
           // OnApplyTemplate();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvasPart = GetTemplateChild("PART_Canvas") as Canvas;
            _scrollViewerPart = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;

            // wire mouse move events at control level to ensure dragging works
            if (_canvasPart != null)
            {
                _canvasPart.SizeChanged += (s, e) => UpdateCanvasSize();
            }

            DrawGraph();
        }

        
         

        // Optional: initialize from a root INode (same behavior as previous NoteGraphControl constructor)
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
            // redraw when nodes collection changes
            Dispatcher.BeginInvoke((Action)(() => DrawGraph()));
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
                Fill = Brushes.LightBlue,
                Stroke = Brushes.DarkBlue,
                StrokeThickness = 2,
                Tag = node
            };

            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);

            ellipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
            ellipse.MouseMove += Ellipse_MouseMove;
            ellipse.MouseLeftButtonUp += Ellipse_MouseLeftButtonUp;

            return ellipse;
        }

        private void DrawEdge(Ellipse parent, Ellipse child)
        {
            if (_canvasPart == null) return;

            var line = new Line
            {
                Stroke = Brushes.Gray,
                StrokeThickness = 2,
                X1 = Canvas.GetLeft(parent) + parent.Width / 2,
                Y1 = Canvas.GetTop(parent) + parent.Height / 2,
                X2 = Canvas.GetLeft(child) + child.Width / 2,
                Y2 = Canvas.GetTop(child) + child.Height / 2
            };
            // Insert lines before ellipses so lines render under nodes
            _canvasPart.Children.Insert(0, line);
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _draggedEllipse = sender as Ellipse;
            if (_draggedEllipse == null || _canvasPart == null) return;

            _dragStart = e.GetPosition(_canvasPart);
            _draggedEllipse.CaptureMouse();
            e.Handled = true;
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggedEllipse != null && e.LeftButton == MouseButtonState.Pressed && _canvasPart != null)
            {
                var pos = e.GetPosition(_canvasPart);
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

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_draggedEllipse != null)
            {
                _draggedEllipse.ReleaseMouseCapture();
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

            _canvasPart.Width = System.Math.Max(contentWidth, minWidth);
            _canvasPart.Height = System.Math.Max(contentHeight, minHeight);
        }

        // Expose canvas if external code needs to manipulate it
        public Canvas? CanvasPart => _canvasPart;
    }
}
open System
open System.Drawing
open System.Windows.Forms

// Create a form to display the graphics
let (width, height) = (500, 500)
let form = new Form(Width = width, Height = height)
let box = new PictureBox(BackColor = Color.White, Dock = DockStyle.Fill)
let image = new Bitmap(width, height)
let graphics = Graphics.FromImage(image)
//The following line produces higher quality images, 
//at the expense of speed. Uncomment it if you want
//more beautiful images, even if it's slower.
//Thanks to https://twitter.com/AlexKozhemiakin for the tip!
//graphics.SmoothingMode <- System.Drawing.Drawing2D.SmoothingMode.HighQuality
let brush = new SolidBrush(Color.FromArgb(0, 0, 0))

box.Image <- image
form.Controls.Add(box) 

// Compute the endpoint of a line
// starting at x, y, going at a certain angle
// for a certain length. 
let endpoint x y angle length =
    x + length * cos angle,
    y + length * sin angle

let flip x = (float)height - x

// Utility function: draw a line of given width, 
// starting from x, y
// going at a certain angle, for a certain length.
let drawLine (target : Graphics) (brush : Brush) (x : float) (y : float) (angle : float) (length : float) (width : float) =
    let x_end, y_end = endpoint x y angle length
    let origin = new PointF((single)x, (single)(y |> flip))
    let destination = new PointF((single)x_end, (single)(y_end |> flip))
    let pen = new Pen(brush, (single)width)
    target.DrawLine(pen, origin, destination)

let draw x y angle length width = 
    drawLine graphics brush x y angle length width

let pi = Math.PI

let (startX, startY) = (250.0, 50.0)
let startAngle = pi * 0.5
let (startLength, startWidth) = (100.0, 4.0)
let leftAngle = pi * 0.3
let rightAngle = -pi * 0.4
let scaleFactor = 0.5
let iterations = 10
// Now... your turn to draw
let rec drawTree x y angle length width leftAngle rightAngle scaleFactor height =
    if height > 0 then
        // The trunk
        draw x y angle length width
        let (branchX, branchY) = endpoint x y angle length
        let (branchLength, branchWidth) = (scaleFactor * length, scaleFactor * width)
        // left and right branches
        draw branchX branchY (angle + leftAngle) branchLength branchWidth
        draw branchX branchY (angle + rightAngle) branchLength branchWidth
        drawTree branchX branchY (angle + leftAngle) branchLength branchWidth leftAngle rightAngle scaleFactor (height - 1)
        drawTree branchX branchY (angle + rightAngle) branchLength branchWidth leftAngle rightAngle scaleFactor (height - 1)
    else
        ignore()
drawTree startX startY startAngle startLength startWidth leftAngle rightAngle scaleFactor iterations
form.ShowDialog()
(* To do a nice fractal tree, using recursion is
probably a good idea. The following link might
come in handy if you have never used recursion in F#:
http://en.wikibooks.org/wiki/F_Sharp_Programming/Recursion
*)
public class Program
{
    public static void Main()
    {
        // Build a sample tree:
        //
        //         4
        //       /   \
        //      2     7
        //     / \   /
        //    1   3 6
        //
        TreeNode root = new TreeNode(4,
            new TreeNode(2,
                new TreeNode(1),
                new TreeNode(3)
            ),
            new TreeNode(7,
                new TreeNode(6),
                null
            )
        );

        System.Console.WriteLine("BFS");
        BFSWalk(root);
        System.Console.WriteLine("DFS");
        DFSWalk(root);
        System.Console.WriteLine("DFS");
        DFSWalkIterative(root);
        System.Console.WriteLine("Inverted BFS");
        BFSWalk(InvertTreeBFS(root));
        Console.WriteLine("Depth DFS (target = 3): " + FindDepthDFS(root, 3));
        Console.WriteLine("Depth BFS (target = 3): " + FindDepthBFS(root, 3));
    }

    static void BFSWalk(TreeNode treeNode)
    {
        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(treeNode);
        while (queue.Count > 0)
        {
            TreeNode current = queue.Dequeue();
            System.Console.WriteLine(current.val);
            if (current.left is not null) queue.Enqueue(current.left);
            if (current.right is not null) queue.Enqueue(current.right);
        }
    }

    static void DFSWalk(TreeNode treeNode)
    {
        if (treeNode == null)
        {
            return;
        }
        System.Console.WriteLine(treeNode.val);
        DFSWalk(treeNode.left);
        DFSWalk(treeNode.right);
    }

    static void DFSWalkIterative(TreeNode treeNode)
    {
        Stack<TreeNode> stack = new Stack<TreeNode>([treeNode]);
        while (stack.Count > 0)
        {
            TreeNode current = stack.Pop();
            System.Console.WriteLine(current.val);
            if (current.right is not null)
            {
                stack.Push(current.right);
            }
            if (current.left is not null)
            {
                stack.Push(current.left);
            }
        }
    }

    static TreeNode InvertTreeBFS(TreeNode treeNode)
    {
        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(treeNode);
        while (queue.Count > 0)
        {
            TreeNode current = queue.Dequeue();
            (current.right, current.left) = (current.left, current.right);
            if (current.left is not null) queue.Enqueue(current.left);
            if (current.right is not null) queue.Enqueue(current.right);
        }
        return treeNode;
    }

    // TODO; iterative
    static TreeNode? InvertTreeDFS(TreeNode treeNode)
    {
        if (treeNode is null)
        {
            return null;
        }
        InvertTreeDFS(treeNode.left);
        InvertTreeDFS(treeNode.right);
        (treeNode.left, treeNode.right) = (treeNode.right, treeNode.left);
        return treeNode;
    }

    static int FindDepthBFS(TreeNode root, int target)
    {
        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        int depth = 0;
        while (queue.Count > 0)
        {
            int length = queue.Count;
            for (int i = 0; i < length; i++)
            {
                TreeNode current = queue.Dequeue();
                if (current.val == target)
                {
                    return depth;
                }
                if (current.left is not null) queue.Enqueue(current.left);
                 if (current.right is not null) queue.Enqueue(current.right);
            }
            depth++;
        }
        return -1;
    }

    static int FindDepthDFS(TreeNode root, int target, int depth=0)
    {
        if (root is null)
        {
            return -1;
        }
        if (root.val == target)
        {
            return depth;
        }
        int left = FindDepthDFS(root.left, target, depth + 1);
        if (left != -1)
        {
            return left;
        }
        int right = FindDepthDFS(root.right, target, depth + 1);
        if (right != -1)
        {
            return right;
        }
        return -1;
    }

}

public class TreeNode {
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
        this.val = val;
        this.left = left;
        this.right = right;
    }
}
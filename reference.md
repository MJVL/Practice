https://www.hellointerview.com/learn/code/two-pointers/overview

groking the coding interview


# BFS / DFS
```CSharp
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
        System.Console.WriteLine("Inverted BFS");
        BFSWalk(InvertTreeBFS(root));
    }

    static void BFSWalk(TreeNode treeNode)
    {
        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(treeNode);
        while (queue.Count != 0)
        {
            TreeNode current = queue.Dequeue();
            System.Console.WriteLine(current.val);
            if (current.left is not null) queue.Enqueue(current.left);
            if (current.right is not null) queue.Enqueue(current.right);
        }
    }

    static void DFSWalk(TreeNode treeNode)
    {
        if (treeNode is null)
        {
            return;
        }
        System.Console.WriteLine(treeNode.val);
        DFSWalk(treeNode.left);
        DFSWalk(treeNode.right);
    }

    static TreeNode InvertTreeBFS(TreeNode treeNode)
    {
        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(treeNode);
        while (queue.Count != 0)
        {
            TreeNode current = queue.Dequeue();
            TreeNode right = current.right;
            current.right = current.left;
            current.left = right;
            if (current.left is not null) queue.Enqueue(current.left);
            if (current.right is not null) queue.Enqueue(current.right);
        }
        return treeNode;
    }

    // TODO; iterative
    static TreeNode InvertTreeDFS(TreeNode treeNode)
    {
        if (treeNode is null)
        {
            return null;
        }
        InvertTreeDFS(treeNode.left);
        InvertTreeDFS(treeNode.right);
        TreeNode temp = treeNode.left;
        treeNode.left = treeNode.right;
        treeNode.right = temp;
        return treeNode;
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
```
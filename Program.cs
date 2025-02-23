using System;
using System.Collections.Generic;

public class TreeNode 
{
    public int Key { get; set; }
    public string Value {get; set;}
    public TreeNode Left { get; set;}
    public TreeNode Right { get; set;}
    public int Height { get; set; }

    public TreeNode(int key, string value)
    {
        Key = key;
        Value = value;
        Height = 1;
    }
}

public class AVLTree
{
    private TreeNode root;

    private int Height(TreeNode node)
    {
        if (node == null)
            return 0;
        return node.Height;
    }

    private void UpdateHeight(TreeNode node)
    {
        if (node != null)
            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
    }

    private int BalanceFactor(TreeNode node)
    {
        if (node == null)
            return 0;
        return Height(node.Left) - Height(node.Right);
    }

    // Right rotation
    private TreeNode RotateRight(TreeNode y)
    {
        TreeNode x = y.Left;
        TreeNode T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        UpdateHeight(y);
        UpdateHeight(x);

        return x;
    }

    // Left rotation
    private TreeNode RotateLeft(TreeNode x)
    {
        TreeNode y = x.Right;
        TreeNode T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        UpdateHeight(x);
        UpdateHeight(y);

        return y;
    }

    // Insert a new node
    public void Insert(int key, string value)
    {
        root = Insert(root, key, value);
    }

    private TreeNode Insert(TreeNode node, int key, string value)
    {
        if (node == null)
        return new TreeNode(key, value);

        if (key < node.Key)
            node.Left = Insert(node.Left, key, value);
        else if (key > node.Key)
            node.Right = Insert(node.Right, key, value);
        else
            return node;

            UpdateHeight(node);

            int balance = BalanceFactor(node);

            // Left Left Case 
            if (balance > 1 && key < node.Left.Key)
                return RotateRight(node);

            // Right Right Case
            if (balance < -1 && key > node.Right.Key)
                return RotateLeft(node);

            // Left Right Case 
            if (balance > 1 && key > node.Left.Key)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            } 

            // Right Left Case
            if (balance < -1 && key < node.Right.Key)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
    }

    // Delete a node
    public void Delete(int key)
    {
        root = Delete(root, key);
    }

    private TreeNode Delete(TreeNode node, int key)
    {
        if (node == null)
            return node;

            if (key < node.Key)
            node.Left = Delete(node.Left, key);
            else if (key > node.Key)
            node.Right = Delete(node.Right, key);
            else
            {
                if (node.Left == null || node.Right == null)
                {
                    TreeNode temp = null;
                    if (node.Left == null)
                        temp = node.Right;
                    else
                        temp = node.Left;


                    if (temp == null)
                    {
                        temp = node;
                        node = null;
                    }
                    else
                    {
                        node = temp;
                    }
                }
                else
                {
                    TreeNode temp = MinValueNode(node.Right);

                    node.Key = temp.Key;

                    node.Right = Delete(node.Right, temp.Key);
                }
            }

            if (node == null)
                return node;

                UpdateHeight(node);

                int balance = BalanceFactor(node);

                // Left Left Case
                if (balance > 1 && BalanceFactor(node.Left) >= 0)
                    return RotateRight(node);

                // Left Right Case
                if (balance > 1 && BalanceFactor(node.Left) < 0)
                {
                    node.Left = RotateLeft(node.Left);
                    return RotateRight(node);
                }

                // Right Right Case
                if (balance < -1 && BalanceFactor(node.Right) <= 0)
                    return RotateLeft(node);

                // Right Left Case
                if (balance < -1 && BalanceFactor(node.Right) > 0)
                {
                    node.Right = RotateRight(node.Right);
                    return RotateLeft(node);
                }

                return node;
    }

    private TreeNode MinValueNode(TreeNode node)
    {
        TreeNode current = node;

        while (current.Left != null)
            current = current.Left;

        return current;
    }

    public void Update(int key, string value)
    {
        root = Update(root, key, value);
    }

    private TreeNode Update(TreeNode node, int key, string value)
    {
        if (node == null || node.Key == key)
            return node;

        if (key < node.Key)
            node.Left = Update(node.Left, key, value);
        else
            node.Right = Update(node.Right, key, value);

        return node;
    }

    private TreeNode Search(TreeNode node, int key)
    {
        if (node == null || node.Key == key)
            return node;

        if (key < node.Key)
            return Search(node.Left, key);
        else
            return Search(node.Right, key);
    }

    public string Search(int key)
    {
        TreeNode node = Search(root, key);
        if (node != null)
            return node.Value;
        return null;
    }

    public List<TreeNode> SearchNodes(int requiredCount)
    {
        List<TreeNode> result = new List<TreeNode>();
        InOrderTraversal(root, result, ref requiredCount);
        return result;
    }

    private void InOrderTraversal(TreeNode node, List<TreeNode> result, ref int requiredCount)
    {
        if (node == null || requiredCount <= 0)
            return;

        InOrderTraversal(node.Left, result, ref requiredCount);

        if (requiredCount > 0)
        {
            result.Add(node);
            requiredCount--;
        }

        InOrderTraversal(node.Right, result, ref requiredCount);
    }

    public bool IsBalanced()
    {
        return IsBalanced(root);
    }

    private bool IsBalanced(TreeNode node)
    {
        if (node == null)
            return true;

        int balanceFactor = BalanceFactor(node);

        if (Math.Abs(balanceFactor) > 1)
            return false;

        return IsBalanced(node.Left) && IsBalanced(node.Right);
    }

    public void PrintTreeWithLines()
    {
        PrintTreeWithLines(root, 0, "");
    }

        private void PrintTreeWithLines(TreeNode node, int space, string prefix)
    {
        if (node == null)
            return;

        space += 10;

        PrintTreeWithLines(node.Right, space, "/");

        Console.WriteLine();
        for (int i = 10; i < space; i++)
            Console.Write(" ");
        Console.WriteLine(prefix + node.Key + " (" + node.Value + ")");

        PrintTreeWithLines(node.Left, space, "\\");
    }

}

class Program
{
    static void Main()
    {
        AVLTree tree = new AVLTree();

        for( int i = 0; i <= 500; i++)
        {
            tree.Insert(i, "Value" + i);
        }

    //    Insert nodes
        // tree.Insert(10, "Value10");
        // tree.Insert(20, "Value20");
        // tree.Insert(30, "Value30");
        // tree.Insert(40, "Value40");
        // tree.Insert(50, "Value50");
        // tree.Insert(25, "Value25");


        Console.WriteLine("Tree with proper lines after insertion:");
        tree.PrintTreeWithLines();

        // Update a node
        tree.Update(30, "UpdatedValue30");
        Console.WriteLine("\nTree with proper lines after updating key 30:");
        tree.PrintTreeWithLines();
 
        // Delete a node

        for( int i = 100; i <= 300; i++)
        {
            tree.Delete(i);
        }
        // tree.Delete(20);
        // Console.WriteLine("\nTree with proper lines after deleting key 20:");
        tree.PrintTreeWithLines();

        // Search for the required number of nodes
        int requiredCount = 2;
        List<TreeNode> nodes = tree.SearchNodes(requiredCount);
        Console.WriteLine($"\nFirst {requiredCount} nodes in the tree:");
        foreach (var node in nodes)
        {
            Console.WriteLine($"{node.Key} ({node.Value})");
        }

        // Search for nodes with keys 300 and 400
        string value300 = tree.Search(300);
        string value400 = tree.Search(400);
        Console.WriteLine($"\nValue of node with key 300: {value300}");
        Console.WriteLine($"\nValue of node with key 400: {value400}");

        // Check if the tree is balanced
        bool isBalanced = tree.IsBalanced();
        Console.WriteLine($"\nIs the AVL tree balanced? {isBalanced}");
    }
}
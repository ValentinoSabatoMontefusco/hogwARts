using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    // Quadtree node
    public class QuadTreeNode
    {
        public Rect boundary; // Boundary of the node
        public List<Vector2> points; // Points stored in the node
        public QuadTreeNode[] children; // Children nodes (subdivisions)

        public QuadTreeNode(Rect boundary)
        {
            this.boundary = boundary;
            points = new List<Vector2>();
            children = new QuadTreeNode[4];
        }
    }

    private QuadTreeNode root; // Root node of the quadtree
    private int capacity; // Maximum capacity of points in a node before subdividing
    private int count;
    public int Count { get => count; }

    public QuadTree(Rect boundary, int capacity)
    {
        root = new QuadTreeNode(boundary);
        this.capacity = capacity;
        this.count = 0;
    }

    // Insert a point into the quadtree
    public void Insert(Vector2 point)
    {
        Insert(root, point);
        count++;
    }

    private void Insert(QuadTreeNode node, Vector2 point)
    {
        // Check if the point is inside the node's boundary
        if (!node.boundary.Contains(point))
        {
            return;
        }

        // If the node is not subdivided and has space, add the point to it
        if (node.points.Count < capacity && node.children[0] == null)
        {
            node.points.Add(point);
            return;
        }

        // If the node is not subdivided and capacity is reached, subdivide it
        if (node.children[0] == null)
        {
            Subdivide(node);
        }

        // Recursively insert the point into the appropriate child node
        for (int i = 0; i < 4; i++)
        {
            Insert(node.children[i], point);
        }
    }

    // Subdivide a node into four children
    private void Subdivide(QuadTreeNode node)
    {
        float x = node.boundary.x;
        float y = node.boundary.y;
        float w = node.boundary.width * 0.5f;
        float h = node.boundary.height * 0.5f;

        // Create child nodes with appropriate boundaries
        node.children[0] = new QuadTreeNode(new Rect(x + w, y + h, w, h)); // NE
        node.children[1] = new QuadTreeNode(new Rect(x, y + h, w, h));     // NW
        node.children[2] = new QuadTreeNode(new Rect(x, y, w, h)); // SW
        node.children[3] = new QuadTreeNode(new Rect(x + w, y, w, h)); // SE

        // Move points from parent node to children nodes
        for (int i = 0; i < node.points.Count; i++)
        {
            Vector2 point = node.points[i];
            Insert(node, point);
        }

        // Clear points from parent node
        node.points.Clear();
    }

    // Find nearest neighbor for a given point
    public Vector2 FindNearestNeighbor(Vector2 point)
    {
        return FindNearestNeighbor(root, point, float.MaxValue, Vector2.zero);
    }

    private Vector2 FindNearestNeighbor(QuadTreeNode node, Vector2 point, float minDistance, Vector2 nearest)
    {

        //If node is null or empty leaf or in remote region, return nearest
        if (node == null || (node.children[0] == null && node.points.Count == 0) ||
            Utilities.SquaredDistanceToRect(point, node.boundary) >= minDistance)
            return nearest;

        if (node.points.Count != 0)
        {
            // Check if any point in the node is closer than the current nearest point
            foreach (Vector2 p in node.points)
            {
                float distance = Vector2.SqrMagnitude(p - point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = p;
                }


            }
        }
        else
        {
            Vector2 candidate;
            float candidateDistance;
            // Recursively search children nodes for a potentially closer point
            for (int i = 0; i < 4; i++)
            {
                candidate = FindNearestNeighbor(node.children[i], point, minDistance, nearest);
                candidateDistance = Vector2.SqrMagnitude(point - candidate);
                if (candidateDistance < minDistance)
                {
                    minDistance = candidateDistance;
                    nearest = candidate;
                }
            }
        }


        return nearest;



    }
}

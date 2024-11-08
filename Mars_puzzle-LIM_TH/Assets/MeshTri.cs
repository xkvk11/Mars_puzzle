using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTri : MonoBehaviour
{
    private void Start()
    {
        // ������ ���۵� ���� �����Ϳ��� �޽��� ����
        CreateTriangleMesh();
    }

    private void CreateTriangleMesh()
    {
        // �̹� ������ ��� �ߺ� ���� ����
        if (GetComponent<MeshFilter>() != null) return;

        // �ﰢ�� �޽��� �����ϰ� �޽� ���Ϳ� �������� �߰��մϴ�.
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // ���ο� �޽� ����
        Mesh mesh = new Mesh();

        // �ﰢ���� ���Ī ������ ��ǥ ����
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),   // ù ��° ������
            new Vector3(1, 0, 0.2f), // �� ��° ������ (���Ī)
            new Vector3(0.5f, 1, 0)  // �� ��° ������
        };

        // �ﰢ�� �� ����
        int[] triangles = new int[]
        {
            0, 1, 2  // �ﰢ���� �����ϴ� �ε���
        };

        // UV ��ǥ ����
        Vector2[] uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0.5f, 1)
        };

        // �޽��� ��ǥ�� ��, UV ������ ����
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        // ��� ��� (���� ������ ����)
        mesh.RecalculateNormals();

        // �޽� ���Ϳ� ������ �޽��� �Ҵ�
        meshFilter.mesh = mesh;

        // ��Ƽ���� �Ҵ� (Standard ���̴��� ���� ���� ����)
        if (meshRenderer != null)
        {
            meshRenderer.material = new Material(Shader.Find("Unlit/Color"));
            meshRenderer.material.color = new Color(0.8f, 0.6f, 0.4f, 0.5f); // ���� ����
        }

        Debug.Log("���Ī �ﰢ�� Mesh ���� �Ϸ�");
    }

    // �����Ϳ��� ��ü�� �����ǰų� �ʱ�ȭ�� �� ���� ������Ʈ�� ��������
    private void OnDestroy()
    {
        if (Application.isEditor)
        {
            DestroyImmediate(GetComponent<MeshFilter>());
            DestroyImmediate(GetComponent<MeshRenderer>());
        }
    }
}

#include <iostream>
#include <fstream>
#include <vector>
#include <stack>

using namespace std;

// Преобразование ориентированного графа в неориентированный
void makeUndirectedGraph(vector<vector<int>>& graph, int n) {
    for (int i = 0; i < n; ++i) {
        for (int j = 0; j < n; ++j) {
            if (graph[i][j] == 1) {
                graph[j][i] = 1;
            }
        }
    }
}

// Поиск в глубину для нахождения компоненты связности
void dfs(int v, vector<vector<int>>& graph, vector<bool>& visited, vector<int>& component) {
    stack<int> stk;
    stk.push(v);
    visited[v] = true;

    while (!stk.empty()) {
        int node = stk.top();
        stk.pop();
        component.push_back(node);

        for (int i = 0; i < graph.size(); ++i) {
            if (graph[node][i] == 1 && !visited[i]) {
                visited[i] = true;
                stk.push(i);
            }
        }
    }
}

// Функция для вычисления матрицы достижимости (Алгоритм Флойда-Уоршелла)
void computeReachabilityMatrix(vector<vector<int>>& graph, vector<vector<int>>& reachability, int n) {
    reachability = graph;
    for (int k = 0; k < n; ++k) {
        for (int i = 0; i < n; ++i) {
            for (int j = 0; j < n; ++j) {
                if (reachability[i][k] && reachability[k][j]) {
                    reachability[i][j] = 1;
                }
            }
        }
    }
}

void readMatrixFromFile(const string& filename, vector<vector<int>>& matrix, int n) {
    ifstream file(filename);
    if (!file.is_open()) {
        cerr << "Ошибка при открытии файла!" << endl;
        exit(1);
    }

    for (int i = 0; i < n; ++i) {
        for (int j = 0; j < n; ++j) {
            file >> matrix[i][j];
        }
    }
    file.close();
}

void printMatrix(const vector<vector<int>>& matrix, int n, const string& message) {
    cout << message << endl;
    for (int i = 0; i < n; ++i) {
        for (int j = 0; j < n; ++j) {
            cout << matrix[i][j] << " ";
        }
        cout << endl;
    }
}

int main() {
    setlocale(LC_ALL, "Russian");
    int n = 10;
    vector<vector<int>> graph(n, vector<int>(n));

    string filename = "graph.txt";
    readMatrixFromFile(filename, graph, n);

    printMatrix(graph, n, "Исходная матрица смежности:");

    makeUndirectedGraph(graph, n);

    vector<bool> visited(n, false);

    vector<vector<int>> components;

    for (int i = 0; i < n; ++i) {
        if (!visited[i]) {
            vector<int> component;
            dfs(i, graph, visited, component);
            components.push_back(component);
        }
    }

    cout << "\nКоличество компонент связности: " << components.size() << "\n";
    for (int i = 0; i < components.size(); ++i) {
        cout << "Компонента " << i + 1 << ": ";
        for (int v : components[i]) {
            cout << v + 1 << " ";
        }
        cout << endl;
    }

    vector<vector<int>> reachability(n, vector<int>(n, 0));
    computeReachabilityMatrix(graph, reachability, n);
    printMatrix(reachability, n, "\nМатрица достижимости:");

    return 0;
}

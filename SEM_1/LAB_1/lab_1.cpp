#include <iostream>
#include <vector>
#include <algorithm>
#include <cstdlib>
#include <ctime>
using namespace std;

const int UNIVERSE_MIN = -50; // Нижняя граница универсума
const int UNIVERSE_MAX = 50;  // Верхняя граница универсума

// Функция для вывода множества
void printSet(const std::vector<int>& set) {
    if (set.empty()) {
        std::cout << "Пустое множество" << std::endl;
        return;
    }

    std::cout << "{ ";
    for (int elem : set) {
        std::cout << elem << " ";
    }
    std::cout << "}" << std::endl;
}

// Функция для создания случайного множества
std::vector<int> generateRandomSet(int numElements) {
    std::vector<int> set;
    srand(static_cast<unsigned>(time(0)));

    while (set.size() < numElements) {
        int elem = UNIVERSE_MIN + rand() % (UNIVERSE_MAX - UNIVERSE_MIN + 1);
        if (std::find(set.begin(), set.end(), elem) == set.end()) {
            set.push_back(elem);
        }
    }

    return set;
}

// Функция для создания множества вручную
std::vector<int> createSetManually() {
    int size;
    std::cout << "Введите количество элементов множества: ";
    std::cin >> size;

    std::vector<int> set;
    for (int i = 0; i < size; ++i) {
        int elem;
        std::cout << "Введите элемент #" << i + 1 << ": ";
        std::cin >> elem;

        if (elem >= UNIVERSE_MIN && elem <= UNIVERSE_MAX) {
            set.push_back(elem);
        }
        else {
            std::cout << "Элемент вне диапазона универсума [-50, 50], попробуйте еще раз." << std::endl;
            --i;
        }
    }
    return set;
}

// Операции над множествами
std::vector<int> unionSets(const std::vector<int>& setA, const std::vector<int>& setB) {
    std::vector<int> result = setA;
    for (int elem : setB) {
        if (std::find(result.begin(), result.end(), elem) == result.end()) {
            result.push_back(elem);
        }
    }
    return result;
}

std::vector<int> intersectionSets(const std::vector<int>& setA, const std::vector<int>& setB) {
    std::vector<int> result;
    for (int elem : setA) {
        if (std::find(setB.begin(), setB.end(), elem) != setB.end()) {
            result.push_back(elem);
        }
    }
    return result;
}

// Функция для создания множества по условиям
std::vector<int> createSetByConditions() {
    std::vector<int> set;
    char sign;
    int multiple;
    int minRange, maxRange;

    // Получение условий от пользователя
    std::cout << "Задание знака (+ или -): ";
    std::cin >> sign;

    std::cout << "Задание кратности числу: ";
    std::cin >> multiple;

    std::cout << "Задание диапазона (минимум и максимум в пределах [-50, 50]): ";
    std::cin >> minRange >> maxRange;

    // Проверка на правильность диапазона
    if (minRange < UNIVERSE_MIN || maxRange > UNIVERSE_MAX || minRange > maxRange) {
        std::cout << "Неверный диапазон!" << std::endl;
        return set; // Возвращаем пустое множество при неверных данных
    }

    // Формирование множества по условиям
    for (int i = minRange; i <= maxRange; ++i) {
        if (i % multiple == 0 && ((sign == '+' && i > 0) || (sign == '-' && i < 0))) {
            set.push_back(i); // Добавляем элементы, соответствующие знаку и кратности
        }
    }

    return set;
}

// Функция для симметрической разности двух множеств
std::vector<int> symmetricDifferenceSets(const std::vector<int>& setA, const std::vector<int>& setB) {
    std::vector<int> result;

    // Добавляем элементы, которые есть в первом множестве, но отсутствуют во втором
    for (int elem : setA) {
        if (std::find(setB.begin(), setB.end(), elem) == setB.end()) {
            result.push_back(elem);
        }
    }

    // Добавляем элементы, которые есть во втором множестве, но отсутствуют в первом
    for (int elem : setB) {
        if (std::find(setA.begin(), setA.end(), elem) == setA.end()) {
            result.push_back(elem);
        }
    }

    return result;
}

std::vector<int> complementToUniverse(const std::vector<int>& set) {
    std::vector<int> result;

    for (int i = UNIVERSE_MIN; i <= UNIVERSE_MAX; ++i) {
        if (std::find(set.begin(), set.end(), i) == set.end()) {
            result.push_back(i);
        }
    }

    return result;
}

// Функция для обработки выражений
std::vector<int> evaluateExpression(const std::vector<std::vector<int>>& sets, char operation, int setIndexA, int setIndexB) {
    std::vector<int> result;

    switch (operation) {
    case '+':
        result = unionSets(sets[setIndexA], sets[setIndexB]);
        std::cout << "Результат объединения: ";
        break;
    case '*':
        result = intersectionSets(sets[setIndexA], sets[setIndexB]);
        std::cout << "Результат пересечения: ";
        break;
    case '^':
        result = symmetricDifferenceSets(sets[setIndexA], sets[setIndexB]);
        std::cout << "Результат симметрической разности: ";
        break;
    case '-':
        result = complementToUniverse(sets[setIndexA]);
        std::cout << "Дополнение до универсума: ";
        break;
    default:
        std::cout << "Неверная операция." << std::endl;
        return result;
    }

    return result;
}

int main() {
    setlocale(LC_ALL, "Russian");
    std::vector<std::vector<int>> sets; // Вектор для хранения всех множеств
    int choice;

    do {
        // Основное меню программы
        std::cout << "\nМеню:\n";
        std::cout << "1. Создать случайное множество\n";
        std::cout << "2. Ввести множество вручную\n";
        std::cout << "3. Создать множество по условиям\n";
        std::cout << "4. Операции над множествами (через выражения)\n";
        std::cout << "5. Выход\n";
        std::cout << "Выберите действие: ";
        std::cin >> choice;

        switch (choice) {
        case 1: {
            int numElements;
            std::cout << "Введите количество элементов: ";
            std::cin >> numElements;
            sets.push_back(generateRandomSet(numElements));
            std::cout << "Сгенерированное множество: ";
            printSet(sets.back());
            break;
        }
        case 2: {
            sets.push_back(createSetManually());
            std::cout << "Введенное множество: ";
            printSet(sets.back());
            break;
        }
        case 3: {
            // Создание множества по условиям
            sets.push_back(createSetByConditions());
            std::cout << "Множество по условиям: ";
            printSet(sets.back());
            break;
        }
        case 4: {
            if (sets.size() < 2) {
                std::cout << "Для выполнения операций нужно как минимум 2 множества." << std::endl;
                break;
            }

            // Ввод выражения
            int setIndexA, setIndexB;
            char operation;
            std::cout << "Введите выражение (например, 0 + 1 для объединения): ";
            std::cin >> setIndexA >> operation >> setIndexB;

            if (setIndexA < 0 || setIndexA >= sets.size() || setIndexB < 0 || setIndexB >= sets.size()) {
                std::cout << "Неверные индексы множеств." << std::endl;
                break;
            }

            std::vector<int> result = evaluateExpression(sets, operation, setIndexA, setIndexB);
            printSet(result);
            break;
        }
        case 5:
            std::cout << "Выход из программы." << std::endl;
            break;
        default:
            std::cout << "Неверный выбор, попробуйте снова." << std::endl;
            break;
        }
    } while (choice != 5);

    return 0;
}

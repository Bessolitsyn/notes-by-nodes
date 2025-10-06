# notes-by-nodes
Simple app  for notes managing
It's my pet-project to learn and practice to be better. 

##The List of Task to implement
##Задачи которые хочу реализовать в NotesByNodes
- Десктопное приложение (.Net MAUI или ReactNative)
- В приложении сделать поле ввода простых команд типа Новая Заметка, вот пример как в Visual Code ()
- Диаграммы из заметок (PlantUML)
- Add comments in English and Rus
- Реализовать поддержку плагинов (например плагины для просмотра заметок) 
- Поиск по заметкам
- Тэгирование, использование стандартных тегов/категорий, плагин навигации по категориям.
- Плагин граф заметок
- Плагин дашборд
- Использовать несколько хранилищ данных (PostgreSQL, MongoDB)
- Плагин для просмотра/реактирования заметок в форматe MD
- Реазизовать кэширование, сейчас это потокобезопасная коллекция в памяти. 
- Реализовать Cloud версию(синхронизация заметок с облачным хранилищем в первую очередь)
- Интегрировать(попытаться) нейросеть для формирования краткого содержания заметки



# Обзор проекта notes-by-nodes

Проект __notes-by-nodes__ - это приложение для управления заметками, построенное на основе онтологического подхода с использованием .NET/C# и WPF.
Демонстрирует применение DDD, Clean Architecture и семантического моделирования в .NET приложении для управления знаниями.

## Архитектура решения

Решение состоит из 7 проектов:

### 1. __notes-by-nodes__ (Ядро)

Основной проект с доменной моделью и бизнес-логикой:

__Доменные сущности__ (основаны на OWL онтологии):

- `Node` - базовый абстрактный класс с свойствами: Uid, Name, Description, Text, Type, CreationDate
- `Note` - заметка (наследует Node), может содержать контент и ссылки на другие заметки
- `Box` - контейнер для заметок (наследует Node)
- `User` - пользователь (наследует Node)
- `Content` - контент заметки
- `File` - файловый контент

__Отношения между сущностями__:

- Иерархическая структура: Node → HasChildNodes → Node
- Владение: User → IsOwnerOf → Node
- Ссылки: Note → HasReference → Note
- Контент: Note → HasContent → Content

__Ключевые слои__:

- `/Entities` - сгенерированные из OWL онтологии классы
- `/Service` - сервисный слой (INoteService, NoteServiceFacade)
- `/UseCases` - интеракторы (CoreInteractor, UserInteractor)
- `/Storage` - интерфейсы хранилищ (INodeStorage, INoteStorage, IBoxStorage)
- `/Ontology` - RDF описание доменной модели

### 2. __OwlToT4templatesTool__

Инструмент для генерации C# классов из OWL онтологии с использованием T4 шаблонов. Автоматизирует создание доменных сущностей на основе семантического описания.

### 3. __EasyFileObjectStorage__

Библиотека для простого файлового хранения объектов:

- `FileStorage` - базовый класс
- `JsonFileStorage` - JSON сериализация

### 4. __StorageAdapters__

Адаптеры для работы с различными хранилищами:

- `NodeStorageAdapter` - базовый адаптер с кешированием (ConcurrentDictionary)
- `LocalNoteStorageAdapter`, `LocalBoxStorageAdapter`, `LocalUserStorageAdapter` - специализированные адаптеры
- `NodeFileStorageProvider` - провайдер хранилищ
- `/Dataset` - классы для сериализации (NodeDataset, NoteDataset, BoxDataset, etc.)

Использует паттерн __Repository__ с асинхронной загрузкой данных.

### 5. __MongoDBStorage__

Модуль для интеграции с MongoDB (альтернативное хранилище).

### 6. __notes-by-nodes-wpfApp__ (Презентационный слой)

WPF приложение с MVVM архитектурой:

__Структура__:

- `MainWindow.xaml/cs` - главное окно
- `MainViewModel` - главная VM с коллекциями Users, NodesTree, Tabs
- `/ViewModel` - модели представления (UserViewModel, NodeTabItem, DialogViewModel)
- `/UserControls` - пользовательские контролы (NoteEditorControl, DialogWindow)
- `/Service` - адаптеры UI (NoteTabItemBuilder, NodeFileStorageAdapter)
- `/Helpers` - вспомогательные классы (DialogManager, CustomTemplateSelector)
- `/Settings` - конфигурация (NotesByNodesSettings)

__Технологии__:

- CommunityToolkit.Mvvm для MVVM
- Dependency Injection (Microsoft.Extensions)
- ObservableCollection для привязки данных

### 7. __TestProject__

Тесты:

- Unit-тесты (EntitiesTests, CoreInteractorTest, MongoStorageUnitTests)
- Компонентные тесты (NoteServiceFacadeTests)
- Тестовые данные в `/FilesStorage`

## Ключевые особенности

1. __Онтология-driven подход__: Доменная модель описана в RDF/OWL, классы генерируются автоматически
2. __Иерархическая структура заметок__: Заметки организованы в виде дерева внутри боксов
3. __Перекрестные ссылки__: Заметки могут ссылаться друг на друга
4. __Гибкое хранение__: Поддержка файлового и MongoDB хранилищ
5. __Асинхронность__: Активное использование async/await
6. __MVVM__: Четкое разделение UI и бизнес-логики

## Поток данных

```javascript
WPF UI → ViewModel → INoteService → CoreInteractor → StorageAdapters → FileStorage/MongoDB
```

## Файловая структура хранилища

Данные хранятся в формате:

- `.luser` - пользователи
- `.lbox` - боксы
- `.lnote` - заметки
- Организовано по подпапкам (например, `/Box` для заметок бокса)

Проект демонстрирует применение DDD, Clean Architecture и семантического моделирования в .NET приложении для управления знаниями.


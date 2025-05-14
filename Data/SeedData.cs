using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Models;

namespace OpenGamedev.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            OpenGamedevContext context,
            UserManager<ApplicationUser> userManager,
            IHostEnvironment env)
        {
            Console.WriteLine($"Инициализация базы данных для окружения: {env.EnvironmentName}");

            await context.Database.MigrateAsync();
            Console.WriteLine("Миграции применены.");

            if (env.IsDevelopment())
            {
                if (!await context.Projects.AnyAsync())
                {
                    Console.WriteLine("Сидирование тестовых данных для разработки...");

                    // --- 1. Создание базовых сущностей (пользователь, категории, рабочие области, проект) ---
                    // Получаем или создаем первого пользователя (для авторов)
                    ApplicationUser? firstUser = await userManager.FindByNameAsync("TestUser");
                    if (firstUser == null)
                    {
                        firstUser = new ApplicationUser
                        {
                            UserName = "TestUser",
                            RegistrationDate = DateTime.UtcNow,
                        };
                        var createResult = await userManager.CreateAsync(firstUser, "TestPassword123!");
                        if (!createResult.Succeeded)
                        {
                            Console.WriteLine("Ошибка при создании тестового пользователя в SeedData:");
                            foreach (var error in createResult.Errors) { Console.WriteLine(error.Description); }
                            throw new InvalidOperationException("Не удалось создать тестового пользователя.");
                        }
                        Console.WriteLine("Создан тестовый пользователь TestUser");
                    }
                    else { Console.WriteLine("Найден существующий пользователь TestUser"); }


                    // Создаем базовые категории задач
                    var categories = new List<FeatureCategory>
                    {
                        new() { Name = "Game Design", Description = "Tasks related to core game concepts, rules, and systems." },
                        new() { Name = "Programming", Description = "Code development for mechanics, AI, tools, etc." },
                        new() { Name = "Assets", Description = "Creation of 3D models, textures, 2D art, animations, UI elements." },
                        new() { Name = "Audio", Description = "Creation of music, sound effects, and voiceovers." },
                        new() { Name = "Narrative", Description = "Writing quests, lore, dialogue, and backstory." },
                    };
                    context.FeatureCategories.AddRange(categories);
                    Console.WriteLine("Созданы базовые категории задач.");

                    // Создаем основные Рабочие Области
                    var workAreas = new List<WorkArea>
                     {
                         new() { Name = "Code: Gameplay Mechanics", Description = "Player actions, combat, interactions logic." },
                         new() { Name = "Assets: Character Art", Description = "Character models, textures, concepts." },
                         new() { Name = "Assets: Character Animation", Description = "Character rigging and animations." },
                         new() { Name = "Story: Main Quest", Description = "Primary storyline, main characters." },
                         new() { Name = "UI: Inventory", Description = "Inventory screen, logic, and UI." },
                         new() { Name = "Code: AI", Description = "Enemy and NPC behavior logic." },
                     };
                    context.WorkAreas.AddRange(workAreas);
                    Console.WriteLine("Созданы основные Рабочие Области.");

                    // Создаем конкретный Пример Проекта
                    var exampleProject = new Project
                    {
                        Title = "Fantasy RPG Demo",
                        Description = "A small demo project for a Fantasy Role-Playing Game, showcasing core mechanics and asset pipeline.",
                        CreationDate = DateTime.UtcNow,
                        RepositoryLink = "https://github.com/OpenGamedev/FantasyRPGDemo"
                    };
                    context.Projects.Add(exampleProject);
                    Console.WriteLine($"Создан проект '{exampleProject.Title}'.");


                    // Сохраняем базовые сущности, чтобы получить их ID для связей
                    await context.SaveChangesAsync();
                    Console.WriteLine("Базовые сущности (пользователь, категории, области, проект) сохранены.");

                    // Получаем созданные сущности для связей (повторно, т.к. возможно были добавлены)
                    var gameDesignCategory = await context.FeatureCategories.FirstOrDefaultAsync(c => c.Name == "Game Design");
                    var programmingCategory = await context.FeatureCategories.FirstOrDefaultAsync(c => c.Name == "Programming");
                    var assetsCategory = await context.FeatureCategories.FirstOrDefaultAsync(c => c.Name == "Assets");
                    var narrativeCategory = await context.FeatureCategories.FirstOrDefaultAsync(c => c.Name == "Narrative");

                    var codeGameplayArea = await context.WorkAreas.FirstOrDefaultAsync(wa => wa.Name == "Code: Gameplay Mechanics");
                    var assetsCharacterArtArea = await context.WorkAreas.FirstOrDefaultAsync(wa => wa.Name == "Assets: Character Art");
                    var assetsCharacterAnimArea = await context.WorkAreas.FirstOrDefaultAsync(wa => wa.Name == "Assets: Character Animation");
                    var storyMainQuestArea = await context.WorkAreas.FirstOrDefaultAsync(wa => wa.Name == "Story: Main Quest");
                    var uiInventoryArea = await context.WorkAreas.FirstOrDefaultAsync(wa => wa.Name == "UI: Inventory");
                    var codeAIArea = await context.WorkAreas.FirstOrDefaultAsync(wa => wa.Name == "Code: AI");



                    var tasks = new List<FeatureRequest>();

                    var designCombatTask = new FeatureRequest
                    {
                        ProjectId = exampleProject.Id,
                        Project = exampleProject,
                        CategoryId = gameDesignCategory?.Id ?? categories.First().Id,
                        Category = gameDesignCategory ?? categories.First(),
                        Title = "Придумать базовый концепт боевой системы (RPG Demo)",
                        Description = "Описать основные правила и механики ближнего боя.",
                        CreationDate = DateTime.UtcNow,
                        Status = FeatureRequestStatus.TaskVoting,
                        AuthorId = firstUser.Id,
                        Author = firstUser,
                        AverageSolutionVotingDuration = TimeSpan.FromDays(5),
                        SolutionVotingStartTime = DateTime.UtcNow.AddDays(7)
                    };
                    tasks.Add(designCombatTask);

                    // Задача 2: Создать 3D модель главного героя (Статус: Proposed)
                    var createHeroModelTask = new FeatureRequest
                    {
                        ProjectId = exampleProject.Id,
                        Project = exampleProject,
                        CategoryId = gameDesignCategory?.Id ?? categories.First().Id,
                        Category = gameDesignCategory ?? categories.First(),
                        Title = "Создать 3D модель главного героя (RPG Demo)",
                        Description = "Высоко- и низкополигональная модель с текстурами.",
                        CreationDate = DateTime.UtcNow.AddMinutes(10),
                        Status = FeatureRequestStatus.Proposed,
                        AuthorId = firstUser.Id,
                        Author = firstUser,
                    };
                    tasks.Add(createHeroModelTask);

                    // Задача 3: Реализовать базовое движение игрока (Статус: Proposed)
                    var implementMovementTask = new FeatureRequest
                    {
                        ProjectId = exampleProject.Id,
                        Project = exampleProject,
                        CategoryId = gameDesignCategory?.Id ?? categories.First().Id,
                        Category = gameDesignCategory ?? categories.First(),
                        Title = "Реализовать базовое движение игрока (RPG Demo)",
                        Description = "Ходьба, бег, поворот.",
                        CreationDate = DateTime.UtcNow.AddMinutes(20),
                        Status = FeatureRequestStatus.Proposed,
                        AuthorId = firstUser.Id,
                        Author = firstUser,
                    };
                    tasks.Add(implementMovementTask);

                    // Задача 4: Написать диалог для первого NPC (Статус: Proposed)
                    var writeFirstNpcDialogueTask = new FeatureRequest
                    {
                        ProjectId = exampleProject.Id,
                        Project = exampleProject,
                        CategoryId = gameDesignCategory?.Id ?? categories.First().Id,
                        Category = gameDesignCategory ?? categories.First(),
                        Title = "Написать диалог для первого NPC (Квестодатель)",
                        Description = "Разработать диалог для персонажа, выдающего первый квест.",
                        CreationDate = DateTime.UtcNow.AddMinutes(30),
                        Status = FeatureRequestStatus.Proposed,
                        AuthorId = firstUser.Id,
                        Author = firstUser,
                    };
                    tasks.Add(writeFirstNpcDialogueTask);

                    // Задача 5: Реализовать анимации движения игрока (Статус: Proposed)
                    var implementAnimationsTask = new FeatureRequest
                    {
                        ProjectId = exampleProject.Id,
                        Project = exampleProject,
                        CategoryId = assetsCategory?.Id ?? categories.First().Id,
                        Category = assetsCategory ?? categories.First(),
                        Title = "Реализовать анимации движения игрока (Ходьба/Бег)",
                        Description = "Встроить анимации в модель и контроллер игрока.",
                        CreationDate = DateTime.UtcNow.AddMinutes(40),
                        Status = FeatureRequestStatus.Proposed,
                        AuthorId = firstUser.Id,
                        Author = firstUser,
                    };
                    tasks.Add(implementAnimationsTask);


                    context.FeatureRequests.AddRange(tasks);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Создано {tasks.Count} примеров задач.");


                    // --- 3. Создание связей FeatureRequestWorkArea (Задача <-> Рабочая Область) ---

                    var taskWorkAreas = new List<FeatureRequestWorkArea>();

                    if (designCombatTask.Id > 0 && codeGameplayArea != null)
                        taskWorkAreas.Add(new FeatureRequestWorkArea { FeatureRequestId = designCombatTask.Id, WorkAreaId = codeGameplayArea.Id });
                    if (designCombatTask.Id > 0 && gameDesignCategory != null && codeGameplayArea != null) // Добавим связь с Game Design, если она есть как WA
                        taskWorkAreas.Add(new FeatureRequestWorkArea { FeatureRequestId = designCombatTask.Id, WorkAreaId = gameDesignCategory.Id }); // Предполагаем, что Category.Id можно использовать как WorkArea.Id если они в одном пуле или есть другая логика

                    if (createHeroModelTask.Id > 0 && assetsCharacterArtArea != null)
                        taskWorkAreas.Add(new FeatureRequestWorkArea { FeatureRequestId = createHeroModelTask.Id, WorkAreaId = assetsCharacterArtArea.Id });

                    if (implementMovementTask.Id > 0 && codeGameplayArea != null)
                        taskWorkAreas.Add(new FeatureRequestWorkArea { FeatureRequestId = implementMovementTask.Id, WorkAreaId = codeGameplayArea.Id });

                    if (writeFirstNpcDialogueTask.Id > 0 && storyMainQuestArea != null)
                        taskWorkAreas.Add(new FeatureRequestWorkArea { FeatureRequestId = writeFirstNpcDialogueTask.Id, WorkAreaId = storyMainQuestArea.Id });

                    if (implementAnimationsTask.Id > 0 && assetsCharacterAnimArea != null)
                        taskWorkAreas.Add(new FeatureRequestWorkArea { FeatureRequestId = implementAnimationsTask.Id, WorkAreaId = assetsCharacterAnimArea.Id });
                    if (implementAnimationsTask.Id > 0 && codeGameplayArea != null)
                        taskWorkAreas.Add(new FeatureRequestWorkArea { FeatureRequestId = implementAnimationsTask.Id, WorkAreaId = codeGameplayArea.Id });

                    context.FeatureRequestWorkAreas.AddRange(taskWorkAreas);
                    Console.WriteLine($"Создано {taskWorkAreas.Count} связей между задачами и рабочими областями.");


                    // --- 4. Создание связей FeatureRequestDependency (Задача -> Зависит от Задачи) ---

                    var dependencies = new List<FeatureRequestDependency>();

                    // implementAnimationsTask зависит от createHeroModelTask
                    if (implementAnimationsTask.Id > 0 && createHeroModelTask.Id > 0)
                    {
                        dependencies.Add(new FeatureRequestDependency
                        {
                            FeatureRequestId = implementAnimationsTask.Id,
                            DependsOnFeatureRequestId = createHeroModelTask.Id
                        });
                    }

                    // implementMovementTask зависит от designCombatTask
                    if (implementMovementTask.Id > 0 && designCombatTask.Id > 0)
                    {
                        dependencies.Add(new FeatureRequestDependency
                        {
                            FeatureRequestId = implementMovementTask.Id,
                            DependsOnFeatureRequestId = designCombatTask.Id
                        });
                    }

                    context.FeatureRequestDependencies.AddRange(dependencies);
                    Console.WriteLine($"Создано {dependencies.Count} связей зависимостей между задачами.");


                    // --- 5. Создание примера Решения для одной из Задач ---
                    // Создадим решение для задачи "Придумать базовый концепт боевой системы" (designCombatTask)
                    if (designCombatTask != null && firstUser != null)
                    {
                        var sampleSolution = new Solution
                        {
                            // Указываем обязательные поля
                            FeatureRequestId = designCombatTask.Id, // Связь с задачей
                            FeatureRequest = designCombatTask, // Навигационное свойство (опционально при установке ID)
                            AuthorId = firstUser.Id,             // ID автора
                            Author = firstUser,                 // Навигационное свойство автора (опционально при установке ID)
                            Description = "Предложен концепт боевой системы с уклонениями и парированием (документ в Notion/GDocs).", // Описание решения

                            // Дополнительные поля для демонстрации workflow решений
                            CreationDate = DateTime.UtcNow.AddDays(1),
                            Status = SolutionStatus.BuildSuccess, // Предполагаем, что оно уже прошло авто-проверки
                            SourceCodeUrl = "http://example.com/combat-concept-doc-link", // Пример ссылки
                            BuildArtifactUrl = null, // Для концепта может не быть артефакта сборки
                            BuildLog = "N/A",
                            BuildDate = DateTime.UtcNow.AddDays(1).AddHours(1)
                        };
                        context.Solutions.Add(sampleSolution);
                        Console.WriteLine($"Создано примерное решение для задачи '{designCombatTask.Title}'.");
                    }


                    // --- Финальное сохранение всех изменений ---
                    await context.SaveChangesAsync();
                    Console.WriteLine("Все сид-данные сохранены.");
                }
                else
                {
                    Console.WriteLine("Пропуск сидирования тестовых данных (база данных не пуста).");
                }
            }
            else
            {
                Console.WriteLine("Сидирование данных для не-Development окружения...");

                // Пример: Сидируем базовую категорию, если их нет
                if (!await context.FeatureCategories.AnyAsync())
                {
                    context.FeatureCategories.Add(new FeatureCategory { Name = "General", Description = "General tasks and requests." });
                    await context.SaveChangesAsync();
                    Console.WriteLine("Сидирована базовая категория 'General'.");
                }
                else
                {
                    Console.WriteLine("Базовые категории уже существуют.");
                }

                // Пример: Сидируем базовые рабочие области, если их нет (если они обязательны)
                if (!await context.WorkAreas.AnyAsync())
                {
                    var basicWorkAreas = new List<WorkArea>
                   {
                        new() { Name = "Unassigned", Description = "Tasks not yet assigned to a specific work area." },
                   };
                    context.WorkAreas.AddRange(basicWorkAreas);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Сидированы базовые рабочие области.");
                }
                else
                {
                    Console.WriteLine("Базовые рабочие области уже существуют.");
                }

                Console.WriteLine("Сидирование данных для не-Development окружения завершено (только базовые сущности).");
            }
        }
    }
}
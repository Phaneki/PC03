# API Inteligente de Tareas y Análisis

Esta es la solución a la Evaluación N.° 3. La API permite gestionar tareas internas y se extenderá con integraciones externas y funcionalidades de ML.NET.

## Requisitos Previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior (actualmente compatible con .NET 10).
- SQLite (viene incluido mediante los paquetes de Entity Framework Core).

## Pasos para ejecutar localmente

1. Clona este repositorio.
2. Abre una terminal y navega hasta la carpeta del proyecto `TaskApi`:
   ```bash
   cd TaskApi
   ```
3. Ejecuta el comando de restauración (opcional, .NET lo hace por defecto al compilar):
   ```bash
   dotnet restore
   ```
4. Ejecuta el proyecto:
   ```bash
   dotnet run
   ```
5. La API estará disponible en `http://localhost:5xxx` (revisa la consola para ver el puerto exacto).

## Comandos de Migración

Para trabajar con la base de datos (Entity Framework Core con SQLite), asegúrate de estar dentro de la carpeta `TaskApi`.

*   **Crear una migración inicial (ya realizada):**
    ```bash
    dotnet ef migrations add InitialCreate
    ```
*   **Actualizar/Crear la base de datos local:**
    ```bash
    dotnet ef database update
    ```

## Endpoints Implementados (Tareas)

La API cuenta con los siguientes endpoints RESTful para la gestión de Tareas:

*   `GET /api/tareas` - Obtiene la lista de tareas. Soporta los siguientes filtros opcionales (búsqueda):
    *   `?estado=Pendiente` (Filtra por estado: Pendiente, EnProceso, Completada)
    *   `?prioridad=Alta` (Filtra por prioridad: Baja, Media, Alta)
    *   `?fechaInicio=2026-05-01&fechaFin=2026-05-31` (Filtra por un rango de fechas de vencimiento)
    *   *Ejemplo combinado:* `GET /api/tareas?estado=Pendiente&prioridad=Alta`
*   `GET /api/tareas/{id}` - Obtiene una tarea específica por su ID.
*   `POST /api/tareas` - Crea una nueva tarea. (Requiere Título, Estado, Prioridad y FechaVencimiento válida).
*   `PUT /api/tareas/{id}` - Actualiza una tarea existente.
*   `DELETE /api/tareas/{id}` - Elimina una tarea por su ID.

## Endpoints Implementados (Tareas Externas)

La API también consume información desde JSONPlaceholder (https://jsonplaceholder.typicode.com/todos). Estos datos se exponen transformados a nuestro propio formato:

*   `GET /api/tareas-externas` - Obtiene la lista completa de tareas externas mapeadas a nuestro DTO (`externalId`, `titulo`, `completado`).
*   `GET /api/tareas-externas/{id}` - Obtiene una tarea externa específica por su ID (devuelve 404 si el ID no existe).

## Endpoints Implementados (Inteligencia Artificial - ML.NET)

La API cuenta con un modelo de Machine Learning entrenado localmente con `Microsoft.ML`. El modelo utiliza el algoritmo de clasificación multiclase `SdcaMaximumEntropy` para evaluar el texto (título y descripción) y predecir qué nivel de prioridad corresponde (Alta, Media o Baja) en base a un dataset predefinido.

*   `POST /api/ml/recomendar-prioridad` - Recibe un JSON con `titulo` y `descripcion` y devuelve la `prioridadRecomendada`. El modelo lee y aprende automáticamente del archivo `ML/priority-dataset.csv`.

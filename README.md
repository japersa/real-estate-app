# 🏠 RealEstateApp

A full-stack real estate management application with:

- **Backend**: .NET 9 + MongoDB
- **Frontend**: React + Vite
- **Containerization**: Docker & Docker Compose

---

## 📦 Requirements

- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [Docker & Docker Compose](https://www.docker.com/)

---

## 🚀 Getting Started

### Run with Docker Compose

```bash
docker-compose up --build
```

- Backend: [http://localhost:8080/swagger](http://localhost:8080/swagger)
- Frontend: [http://localhost:5173](http://localhost:5173)

---

## 📁 Project Structure

```plaintext
/
├── backend/     # ASP.NET Core Web API (.NET 9)
├── frontend/    # React + Vite app
└── docker-compose.yml
```

---

## 🔧 Development (manual)

### Backend

```bash
cd backend
dotnet restore
dotnet run --project RealEstate.API --urls "https://localhost:5241"
```

### Frontend

```bash
cd frontend
npm install
npm run dev
```

---

## 🛠 Environment

### Backend (`appsettings.json` or env vars)

```json
"mongo": {
  "connectionString": "mongodb://localhost:27017"
}
```

### Frontend (`.env`)

```env
VITE_BACKEND_URL=https://localhost:8080
```

---

## ✅ CI Pipeline

A GitHub Actions workflow validates:

- Backend restore/build
- Frontend install/build
- MongoDB availability

Path: `.github/workflows/ci.yml`

---

## 📄 License

MIT
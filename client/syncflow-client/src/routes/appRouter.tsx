import { BrowserRouter, Routes, Route } from "react-router-dom";
import MainLayout from "../components/layout/mainLayout";
import DashboardPage from "../pages/dashboardPage";
import ProjectsPage from "../pages/projectsPage";
import NotFoundPage from "../pages/notFoundPage";
import ProtectedRoute from "../components/protectedRoute";
import LoginPage from "../pages/loginPage";

const AppRouter = () => {
  return (
    <BrowserRouter>
      <MainLayout>
        <Routes>
          <Route path="/" element={<DashboardPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/projects"  element={
            <ProtectedRoute>
              <ProjectsPage />
            </ProtectedRoute>
          } />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </MainLayout>
    </BrowserRouter>
  );
};

export default AppRouter;

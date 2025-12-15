import type { ReactNode } from "react";
import { Link } from "react-router-dom";

interface MainLayoutProps {
  children: ReactNode;
}

const MainLayout = ({ children }: MainLayoutProps) => {
  return (
    <div className="min-h-screen flex flex-col">
      <header className="bg-blue-600 text-white p-4 flex gap-4">
        <Link to="/">Dashboard</Link>
        <Link to="/projects">Projects</Link>
      </header>
      <main className="flex-1 p-4 bg-gray-50">
        {children}
      </main>
    </div>
  );
};

export default MainLayout;

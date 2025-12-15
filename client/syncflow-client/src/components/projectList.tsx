import React, { useState } from "react";
import type { Project } from "../models/project";

interface ProjectsProps {
  projects: Project[];
  onUpdate: (updatedProject: Project) => void;
  onDelete: (id: string) => void;
}

export const ProjectList: React.FC<ProjectsProps> = ({ projects, onUpdate, onDelete }) => {
  const [editingProject, setEditingProject] = useState<Project | null>(null);
  const [editName, setEditName] = useState("");
  const [editDescription, setEditDescription] = useState("");
  const [editStartDate, setEditStartDate] = useState("");
  const [editEndDate, setEditEndDate] = useState("");

  const handleEditClick = (project: Project) => {
    setEditingProject(project);
    setEditName(project.name);
    setEditDescription(project.description || "");
    setEditStartDate(project.startDate ? new Date(project.startDate).toISOString().slice(0, 10) : "");
    setEditEndDate(project.endDate ? new Date(project.endDate).toISOString().slice(0, 10) : "");
  };

const handleSave = async () => {
    if (editingProject) {
        await onUpdate({
            ...editingProject,
            name: editName,
            description: editDescription,
            startDate: new Date(editStartDate),
            endDate: editEndDate ? new Date(editEndDate) : null
        });
        setEditingProject(null);
    }
};

  return (
    <div>
      <h2 className="text-xl font-bold mb-4">Lista de Proyectos</h2>
      {projects.length === 0 && <p>No hay proyectos</p>}
      
      <ul>
        {projects.map((p) => (
          <li key={p.id} className="flex flex-col md:flex-row md:items-center justify-between border p-2 mb-2 rounded">
            {editingProject?.id === p.id ? (
              <div className="flex-1 mr-2 space-y-2">
                <input
                  className="border p-1 w-full"
                  value={editName}
                  onChange={(e) => setEditName(e.target.value)}
                  placeholder="Nombre del proyecto"
                />
                <textarea
                  className="border p-1 w-full"
                  value={editDescription}
                  onChange={(e) => setEditDescription(e.target.value)}
                  placeholder="Descripción"
                />
                <div className="flex gap-2">
                  <input
                    type="date"
                    className="border p-1 w-full"
                    value={editStartDate}
                    onChange={(e) => setEditStartDate(e.target.value)}
                  />
                  <input
                    type="date"
                    className="border p-1 w-full"
                    value={editEndDate}
                    onChange={(e) => setEditEndDate(e.target.value)}
                  />
                </div>
              </div>
            ) : (
              <div className="flex-1">
                <strong>{p.name}</strong>
                <p>{p.description}</p>
                <small className="text-gray-500">
                  {new Date(p.startDate).toLocaleDateString()} –{" "}
                  {p.endDate ? new Date(p.endDate).toLocaleDateString() : "En curso"}
                </small>
              </div>
            )}

            <div className="flex space-x-2 mt-2 md:mt-0">
              {editingProject?.id === p.id ? (
                <>
                  <button onClick={handleSave} className="bg-green-500 text-white px-3 py-1 rounded">
                    Guardar
                  </button>
                  <button onClick={() => setEditingProject(null)} className="bg-gray-500 text-white px-3 py-1 rounded">
                    Cancelar
                  </button>
                </>
              ) : (
                <>
                  <button onClick={() => handleEditClick(p)} className="bg-yellow-500 text-white px-3 py-1 rounded">
                    Editar
                  </button>
                  <button onClick={() => onDelete(p.id)} className="bg-red-500 text-white px-3 py-1 rounded">
                    Eliminar
                  </button>
                </>
              )}
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
};

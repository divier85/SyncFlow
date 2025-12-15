import React, { useEffect, useState } from "react";
import type { Project, ProjectFilter } from "../models/project";
import type { PagedResult } from "../models/pagedResult";
import projectsApi from "../api/projectsApi";
import { ProjectList } from "../components/projectList";

const ProjectsPage: React.FC = () => {
    const [projects, setProjects] = useState<PagedResult<Project>>({
        items: [],
        totalItems: 0,
        page: 1,
        pageSize: 10,
        totalPages: 0
    });

    const [filters, setFilters] = useState<ProjectFilter>({
        page: 1,
        pageSize: 10
    });

    const [newProject, setNewProject] = useState<Project>({
        name: "",
        description: "",
        businessId: "",
        createdAt: new Date(),
        id: "",
        startDate: new Date(),
        endDate: new Date()
    });

    useEffect(() => {
        loadProjects();
    }, [filters]);

    const loadProjects = async () => {
        try {
            const data = await projectsApi.getProjects(filters);
            debugger;
            setProjects(data);
        } catch (error) {
            console.error("Error loading projects", error);
        }
    };

    const handleAdd = async () => {
        if (!newProject.name.trim()) return;
        try {
            await projectsApi.create(newProject);
            setNewProject({
                name: "",
                description: "",
                businessId: "",
                createdAt: new Date(),
                id: "",
                startDate: new Date(),
                endDate: new Date()
            });
            await loadProjects();
        } catch (error) {
            console.error("Error creando proyecto", error);
        }
    };

    const handleUpdate = async (updated: Project) => {
        try {
            await projectsApi.update(updated.id, updated);
            
            setProjects(prev => ({
                ...prev,
                items: prev.items.map(p =>
                    p.id === updated.id ? { ...p, ...updated } : p
                )
            }));

            await loadProjects();
        } catch (error) {
            console.error("Error editando proyecto", error);
        }
    };




    const handleDelete = async (id: string) => {
        try {
            await projectsApi.remove(id);
            await loadProjects();
        } catch (error) {
            console.error("Error eliminando proyecto", error);
        }
    };

    return (
        <div className="max-w-7xl mx-auto px-6 py-8">
            <h1 className="text-3xl font-bold mb-6">Projects</h1>

            {/* Formulario para agregar proyecto */}
            <div className="bg-white p-4 rounded shadow mb-6">
                <input
                    className="border p-2 mr-2"
                    placeholder="Nombre"
                    value={newProject.name}
                    onChange={(e) => setNewProject({ ...newProject, name: e.target.value })}
                />
                <input
                    className="border p-2 mr-2"
                    placeholder="Descripción"
                    value={newProject.description}
                    onChange={(e) =>
                        setNewProject({ ...newProject, description: e.target.value })
                    }
                />
                <button
                    className="bg-blue-500 text-white px-4 py-2 rounded"
                    onClick={handleAdd}
                >
                    Agregar
                </button>
            </div>

            {/* Lista con edición y eliminación */}
            <div className="bg-white rounded-lg shadow p-6">
                <ProjectList
                    projects={projects.items}
                    onUpdate={handleUpdate}
                    onDelete={handleDelete}
                />
            </div>

            <div className="mt-6 flex justify-between items-center">
                <span>
                    Página {projects.page} de {projects.totalPages}
                </span>
                {/* Aquí iría la paginación */}
            </div>
        </div>
    );
};

export default ProjectsPage;

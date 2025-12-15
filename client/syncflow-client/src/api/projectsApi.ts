import type { PagedResult } from "../models/pagedResult";
import type { Project, ProjectFilter } from "../models/project";
import axiosClient from "./axiosClient";

class ProjectsApi {

    async getProjects(filter: ProjectFilter): Promise<PagedResult<Project>> {
        const response = await axiosClient.get<PagedResult<Project>>(`/projects`, {
            params: filter
        });
        return response.data;
    }

    async getById(id: string): Promise<Project> {
        const response = await axiosClient.get(`/projects/${id}`)
        return response.data;
    }

    async create(data: Project) {
        data.businessId='DFFEC6BE-E630-4C4F-A001-157A811AC283';
        axiosClient.post("/projects", data)
    }

    async update(id: string, data: Project) {
        axiosClient.put(`/projects/${id}`, data)
    }
    async remove(id: string) {
        axiosClient.delete(`/projects/${id}`)
    }
}

export default new ProjectsApi();
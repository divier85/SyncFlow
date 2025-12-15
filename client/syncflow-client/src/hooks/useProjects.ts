import { useState, useEffect } from "react";
import ProjectsApi  from "../api/projectsApi";
import type { PagedResult } from "../models/pagedResult";
import type { Project, ProjectFilter } from "../models/project";

export function useProjects(initialFilter: ProjectFilter) {
  const [filter, setFilter] = useState<ProjectFilter>(initialFilter);
  const [data, setData] = useState<PagedResult<Project> | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    async function fetchData() {
      setLoading(true);
      try {
        const result = await ProjectsApi.getProjects(filter);
        setData(result);
      } catch (error) {
        console.error("Error fetching projects", error);
      }
      setLoading(false);
    }
    fetchData();
  }, [filter]);

  return { data, loading, setFilter };
}

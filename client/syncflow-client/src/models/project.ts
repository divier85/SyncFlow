export interface Project {
  id: string;
  name: string;
  description?: string;
  startDate: Date;     // Usamos string por el formato ISO de la API
  endDate?: Date | null;
  createdAt: Date;
  businessId: string;
}

export interface ProjectFilter {
  page: number;
  pageSize: number;
  orderBy?: string[];
  orderDirection?: string[];
  name?: string;
  startDate?: Date; // ISO
  endDate?: Date;   // ISO
}


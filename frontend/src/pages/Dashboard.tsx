import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { apiClient } from "@/lib/api-client";
import { Card } from "@/components/ui/card";
import { SearchPanel } from "@/components/dashboard/SearchPanel";
import { VisitsDataGrid } from "@/components/dashboard/VisitsDataGrid";
import { Button } from "@/components/ui/button";
import { UserPlus } from "lucide-react";
import { useNavigate } from "react-router-dom";

export interface SearchFilters {
  hotelIds: string[];
  monthYear: Date | undefined;
  onlyLoyal: boolean;
}

const Dashboard = () => {
  const navigate = useNavigate();
  const [filters, setFilters] = useState<SearchFilters>({
    hotelIds: [],
    monthYear: undefined,
    onlyLoyal: false,
  });

  const { data: visits, isLoading } = useQuery({
    queryKey: ["visits", filters],
    queryFn: async () => {
      return apiClient.getVisits({
        hotelIds: filters.hotelIds,
        monthYear: filters.monthYear,
        onlyLoyal: filters.onlyLoyal,
      });
    },
  });

  return (
    <div className="min-h-screen bg-background">
      <header className="border-b border-border bg-card shadow-sm">
        <div className="container mx-auto px-4 py-6">
          <div className="flex items-center justify-between">
            <div>
              <h1 className="text-3xl font-bold text-foreground">Hotel Visitation Analytics</h1>
              <p className="text-muted-foreground mt-1">Track and analyze customer visits across all properties</p>
            </div>
            <Button onClick={() => navigate("/profile")} className="gap-2">
              <UserPlus className="h-4 w-4" />
              New Customer
            </Button>
          </div>
        </div>
      </header>

      <main className="container mx-auto px-4 py-8">
        <div className="space-y-6">
          <Card className="p-6">
            <SearchPanel onSearch={setFilters} />
          </Card>

          <Card className="p-6">
            <VisitsDataGrid visits={visits || []} isLoading={isLoading} />
          </Card>
        </div>
      </main>
    </div>
  );
};

export default Dashboard;
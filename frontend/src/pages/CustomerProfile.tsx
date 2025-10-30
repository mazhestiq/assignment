import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "@/lib/api-client";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { ArrowLeft, Calendar, Mail, Phone, User } from "lucide-react";
import { toast } from "sonner";
import { VisitRegistrationModal } from "@/components/customer/VisitRegistrationModal";

const CustomerProfile = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const [showVisitModal, setShowVisitModal] = useState(false);

  const [formData, setFormData] = useState({
    name: "",
    email: "",
    phone: "",
    is_loyal: false,
  });

  const { data: customer, isLoading } = useQuery({
    queryKey: ["customer", id],
    queryFn: async () => {
      if (!id) return null;
      return apiClient.getCustomer(id);
    },
    enabled: !!id,
  });

  const { data: visits } = useQuery({
    queryKey: ["customer-visits", id],
    queryFn: async () => {
      if (!id) return [];
      return apiClient.getCustomerVisits(id);
    },
    enabled: !!id,
  });

  useEffect(() => {
    if (customer) {
      setFormData({
        name: customer.name || "",
        email: customer.email || "",
        phone: customer.phone || "",
        is_loyal: customer.is_loyal || false,
      });
    }
  }, [customer]);

  const createMutation = useMutation({
    mutationFn: async (data: typeof formData) => {
      return apiClient.createCustomer(data);
    },
    onSuccess: (data) => {
      toast.success("Customer profile created successfully");
      queryClient.invalidateQueries({ queryKey: ["customers"] });
      navigate(`/profile/${data.id}`);
    },
    onError: (error: Error) => {
      toast.error(error.message || "Failed to create customer profile");
    },
  });

  const updateMutation = useMutation({
    mutationFn: async (data: typeof formData) => {
      if (!id) throw new Error("No customer ID");
      return apiClient.updateCustomer(id, data);
    },
    onSuccess: () => {
      toast.success("Customer profile updated successfully");
      queryClient.invalidateQueries({ queryKey: ["customer", id] });
    },
    onError: (error: Error) => {
      toast.error(error.message || "Failed to update customer profile");
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (id) {
      updateMutation.mutate(formData);
    } else {
      createMutation.mutate(formData);
    }
  };

  if (id && isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-muted-foreground">Loading customer profile...</div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-background">
      <header className="border-b border-border bg-card shadow-sm">
        <div className="container mx-auto px-4 py-6">
          <div className="flex items-center gap-4">
            <Button variant="ghost" size="icon" onClick={() => navigate("/")}>
              <ArrowLeft className="h-5 w-5" />
            </Button>
            <div>
              <h1 className="text-3xl font-bold text-foreground">
                {id ? "Customer Profile" : "New Customer Registration"}
              </h1>
              <p className="text-muted-foreground mt-1">
                {id ? "View and manage customer information" : "Create a new customer profile"}
              </p>
            </div>
          </div>
        </div>
      </header>

      <main className="container mx-auto px-4 py-8">
        <div className="max-w-4xl mx-auto space-y-6">
          <Card className="p-6">
            <form onSubmit={handleSubmit} className="space-y-6">
              <div className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="name" className="flex items-center gap-2">
                    <User className="h-4 w-4" />
                    Full Name *
                  </Label>
                  <Input
                    id="name"
                    value={formData.name}
                    onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                    placeholder="John Doe"
                    required
                  />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="email" className="flex items-center gap-2">
                    <Mail className="h-4 w-4" />
                    Email
                  </Label>
                  <Input
                    id="email"
                    type="email"
                    value={formData.email}
                    onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                    placeholder="john.doe@example.com"
                  />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="phone" className="flex items-center gap-2">
                    <Phone className="h-4 w-4" />
                    Phone
                  </Label>
                  <Input
                    id="phone"
                    type="tel"
                    value={formData.phone}
                    onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                    placeholder="+1 (555) 000-0000"
                  />
                </div>

                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="loyal"
                    checked={formData.is_loyal}
                    onCheckedChange={(checked) =>
                      setFormData({ ...formData, is_loyal: checked as boolean })
                    }
                  />
                  <Label htmlFor="loyal" className="cursor-pointer">
                    Mark as Loyal Customer
                  </Label>
                </div>
              </div>

              <div className="flex gap-3">
                <Button type="submit" disabled={createMutation.isPending || updateMutation.isPending}>
                  {id ? "Update Profile" : "Create Profile"}
                </Button>
                {id && (
                  <Button
                    type="button"
                    variant="outline"
                    onClick={() => setShowVisitModal(true)}
                    className="gap-2"
                  >
                    <Calendar className="h-4 w-4" />
                    Register Visit
                  </Button>
                )}
              </div>
            </form>
          </Card>

          {id && visits && visits.length > 0 && (
            <Card className="p-6">
              <h2 className="text-xl font-semibold mb-4">Visit History</h2>
              <div className="space-y-3">
                {visits.map((visit) => (
                  <div
                    key={visit.id}
                    className="flex items-center justify-between p-3 rounded-lg bg-muted/50"
                  >
                    <div>
                      <p className="font-medium">{visit.hotel?.name}</p>
                      <p className="text-sm text-muted-foreground">
                        {new Date(visit.visit_date).toLocaleDateString()}
                      </p>
                    </div>
                  </div>
                ))}
              </div>
            </Card>
          )}
        </div>
      </main>

      {id && (
        <VisitRegistrationModal
          open={showVisitModal}
          onOpenChange={setShowVisitModal}
          customerId={id}
        />
      )}
    </div>
  );
};

export default CustomerProfile;
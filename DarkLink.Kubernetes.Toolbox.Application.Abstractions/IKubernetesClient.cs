namespace DarkLink.Kubernetes.Toolbox.Application;

public interface IKubernetesClient<RT> where RT : struct, HasCancel<RT>
{
    Aff<RT, Seq<Pod>> GetPods();
    
    Aff<RT, Seq<PersistentVolumeClaim>> GetPersistentVolumeClaims();
}
